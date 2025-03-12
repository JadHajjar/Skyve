using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class TroubleshootInfoControl : SlickControl
{
	private Rectangle buttonRect;
	private Rectangle cancelRect;
	private Rectangle launchRect;

	private readonly ITroubleshootSystem _troubleshootSystem;
	private readonly ICitiesManager _citiesManager;

	public TroubleshootInfoControl()
	{
		ServiceCenter.Get(out _troubleshootSystem, out _citiesManager);

		_troubleshootSystem.AskForConfirmation += AskForConfirmation;
		_troubleshootSystem.StageChanged += CheckVisibility;
		_troubleshootSystem.PromptResult += PromptResult;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (Loading)
		{
			return;
		}

		if (cancelRect.Contains(e.Location))
		{
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, Locale.TroubleshootCancelTip);
		}
		else if (buttonRect.Contains(e.Location))
		{
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, Locale.TroubleshootNextStageTip);
		}
		else if (launchRect.Contains(e.Location))
		{
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, Locale.LaunchTooltip.Format("[F5]"));
		}
		else
		{
			Cursor = Cursors.Default;
			SlickTip.SetTo(this, Locale.TroubleshootBubbleTip);
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (Live)
		{
			CheckVisibility();
		}
	}

	private void CheckVisibility()
	{
		if (Visible != _troubleshootSystem.IsInProgress)
		{
			this.TryInvoke(() => Visible = _troubleshootSystem.IsInProgress);
		}

		Program.MainForm.CurrentFormState = _troubleshootSystem.IsInProgress ? FormState.Busy : FormState.NormalFocused;

		if (Visible)
		{
			Invalidate();
		}
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(6));
		Height = UI.Scale(110);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!Live)
		{
			return;
		}

		Loading = _troubleshootSystem.IsBusy;

		e.Graphics.SetUp(BackColor);

		e.Graphics.FillRoundedRectangleWithShadow(ClientRectangle.Pad(Padding.Left / 2), Padding.Left, Padding.Left / 2, null, Color.FromArgb(5, FormDesign.Design.OrangeColor), addOutline: true);

		var y = Padding.Top;
		var cursor = PointToClient(Cursor.Position);
		var launchButtonVisible = _troubleshootSystem.WaitingForGameLaunch || (_troubleshootSystem.WaitingForGameClose && CrossIO.CurrentPlatform is Platform.Windows);
		using var smallFont = UI.Font(6.75F);

		buttonRect = SlickButton.AlignAndDraw(e.Graphics, ClientRectangle.Pad(Padding.Left * 3 / 2), ContentAlignment.BottomRight, new ButtonDrawArgs
		{
			Icon = "Skip",
			Text = Locale.NextStage,
			Font = smallFont,
			HoverState = HoverState,
			Padding = UI.Scale(new Padding(3, 2, 2, 1)),
			BorderRadius = Padding.Left / 2,
			ButtonType = launchButtonVisible ? ButtonType.Dimmed : ButtonType.Normal,
			ColorStyle = ColorStyle.Green,
			Enabled = !Loading,
			Cursor = cursor
		});

		cancelRect = SlickButton.AlignAndDraw(e.Graphics, ClientRectangle.Pad(Padding.Left * 3 / 2), ContentAlignment.BottomLeft, new ButtonDrawArgs
		{
			Icon = "Cancel",
			Text = LocaleSlickUI.Cancel,
			Font = smallFont,
			HoverState = HoverState & ~HoverState.Focused,
			Padding = UI.Scale(new Padding(3, 2, 2, 1)),
			BorderRadius = Padding.Left / 2,
			ButtonType = ButtonType.Hidden,
			ColorStyle = ColorStyle.Red,
			Enabled = !Loading,
			Cursor = cursor
		});

		if (_troubleshootSystem.TotalStages > 0)
		{
			DrawProgress(e, ref y, buttonRect.Size);
		}

		using var font = UI.Font(8.25F, FontStyle.Bold);
		using var brush = new SolidBrush(FormDesign.Design.ForeColor);
		var text = _troubleshootSystem.CurrentAction;
		var textSize = e.Graphics.Measure(text, font, Width - (Padding.Horizontal * 2));
		e.Graphics.DrawString(text, font, brush, new Rectangle(Padding.Horizontal, y, Width - (Padding.Horizontal * 2), 2 * (int)textSize.Height));

		y += (int)textSize.Height + Padding.Top;

		if (Loading)
		{
			DrawLoader(e.Graphics, ClientRectangle.Pad(0, y, 0, buttonRect.Height * 2));

			Height = UI.Scale(110);

			return;
		}

		if (launchButtonVisible)
		{
			launchRect = SlickButton.AlignAndDraw(e.Graphics, ClientRectangle.Pad(Padding.Left * 3 / 2).ClipTo(y + Padding.Top), ContentAlignment.BottomLeft, new ButtonDrawArgs
			{
				Icon = "CS",
				Text = LocaleHelper.GetGlobalText(_citiesManager.IsRunning() ? "StopCities" : "StartCities"),
				Font = smallFont,
				RequiredWidth = ClientRectangle.Width - (Padding.Horizontal * 3 / 2),
				HoverState = HoverState & ~HoverState.Focused,
				Padding = new(4, 3, 2, 2),
				BorderRadius = Padding.Left / 2,
				Enabled = !Loading,
				Cursor = cursor
			});

			y += launchRect.Height + Padding.Bottom;
		}

		using var pen = new Pen(Color.FromArgb(80, FormDesign.Design.ForeColor), (float)UI.FontScale);
		e.Graphics.DrawLine(pen, Padding.Horizontal, y, Width - Padding.Horizontal, y);

		y += Padding.Top * 3 / 2;

		Height = y + buttonRect.Height + Padding.Bottom;
	}

	private void DrawProgress(PaintEventArgs e, ref int y, Size buttonSize)
	{
		var text = Locale.StageCounter.Format(_troubleshootSystem.CurrentStage, _troubleshootSystem.TotalStages);
		var barRect = ClientRectangle.Pad(Padding.Left * 3 / 2).ClipTo(buttonSize.Height);
		using var backBarBrush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.MenuColor));
		using var activeBarBrush = new LinearGradientBrush(barRect, FormDesign.Design.OrangeColor, FormDesign.Design.GreenColor, LinearGradientMode.Horizontal);
		using var backTextBrush = new SolidBrush(FormDesign.Design.MenuForeColor);
		using var activeTextBrush = new SolidBrush(FormDesign.Design.ActiveForeColor);

		e.Graphics.FillRoundedRectangle(backBarBrush, barRect, UI.Scale(4));
		e.Graphics.DrawString(text, Font, backTextBrush, barRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

		e.Graphics.SetClip(new Rectangle(Padding.Left, 0, (Width - Padding.Horizontal) * _troubleshootSystem.CurrentStage / _troubleshootSystem.TotalStages, Height));

		e.Graphics.FillRoundedRectangle(activeBarBrush, barRect, UI.Scale(4));
		e.Graphics.DrawString(text, Font, activeTextBrush, barRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

		e.Graphics.ResetClip();

		y += barRect.Height + Padding.Bottom;
	}

	private async void AskForConfirmation()
	{
		var issuePersists = MessagePrompt.Show(Locale.TroubleshootAskIfFixed, LocaleSlickUI.Confirmation, PromptButtons.YesNo, PromptIcons.Question, form: Program.MainForm) == DialogResult.Yes;

		//if (_troubleshootSystem.CurrentStage * 2 >= _troubleshootSystem.TotalStages && MessagePrompt.Show(Locale.TroubleshootAskToStop, Locale.StopTroubleshootTitle, PromptButtons.YesNo, PromptIcons.Hand, form: Program.MainForm) == DialogResult.Yes)
		//{
		//	this.TryInvoke(Hide);
		//	await Task.Run(() => _troubleshootSystem.Stop(true));
		//	return;
		//}

		var applyResult = await _troubleshootSystem.ApplyConfirmation(issuePersists);

		if (applyResult < TroubleshootResult.Error)
		{
			return;
		}

		MessagePrompt.Show(Locale.TroubleshootActionFailed
			+ "\r\n"
			+ LocaleHelper.GetGlobalText($"Troubleshoot{applyResult}"), PromptButtons.OK, PromptIcons.Error, Program.MainForm);
	}

	private void PromptResult(IEnumerable<ILocalPackageIdentity> list)
	{
		if (list.Any())
		{
			MessagePrompt.Show(Locale.TroubleshootCauseResult + "\r\n\r\n" + list.ListStrings("\r\n"), icon: PromptIcons.Ok, form: Program.MainForm);
		}
		else
		{
			MessagePrompt.Show(Locale.TroubleshootInconclusive, icon: PromptIcons.Hand, form: Program.MainForm);
		}
	}

	protected override async void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (Loading)
		{
			return;
		}

		if (e.Button == MouseButtons.None || (e.Button == MouseButtons.Left && buttonRect.Contains(e.Location)))
		{
			if (_troubleshootSystem.WaitingForPrompt)
			{
				AskForConfirmation();
			}
			else if (_troubleshootSystem.WaitingForGameLaunch || _troubleshootSystem.WaitingForGameClose)
			{
				await Task.Run(_troubleshootSystem.GoToNextStage);
			}
		}
		else if (e.Button == MouseButtons.Left && launchRect.Contains(e.Location))
		{
			if (_citiesManager.IsAvailable())
			{
				if (_citiesManager.IsRunning())
				{
					new BackgroundAction("Stopping Cities: Skylines", _citiesManager.Kill).Run();
				}
				else
				{
					new BackgroundAction("Starting Cities: Skylines", _citiesManager.Launch).Run();
				}
			}
		}
		else if (e.Button == MouseButtons.Left && cancelRect.Contains(e.Location))
		{
			TroubleshootResult applyResult;

			switch (MessagePrompt.Show(Locale.CancelTroubleshootMessage, Locale.CancelTroubleshootTitle, PromptButtons.YesNoCancel, PromptIcons.Hand, form: Program.MainForm))
			{
				case DialogResult.Yes:
					applyResult = await _troubleshootSystem.Stop(false);
					Hide();
					break;
				case DialogResult.No:
					applyResult = await _troubleshootSystem.Stop(true);
					Hide();
					break;
				default:
					return;
			}

			if (applyResult < TroubleshootResult.Error)
			{
				return;
			}

			MessagePrompt.Show(Locale.TroubleshootActionFailed
				+ "\r\n"
				+ LocaleHelper.GetGlobalText($"Troubleshoot{applyResult}"), PromptButtons.OK, PromptIcons.Error, Program.MainForm);
		}
	}
}
