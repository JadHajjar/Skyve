using System.Drawing;
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
		var system = ServiceCenter.Get<ITroubleshootSystem>();

		_citiesManager = ServiceCenter.Get<ICitiesManager>();
		_troubleshootSystem = system;

		if (system != null)
		{
			system.AskForConfirmation += AskForConfirmation;
			system.StageChanged += CheckVisibility;
			system.PromptResult += PromptResult;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (cancelRect.Contains(e.Location))
		{
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, "TroubleshootCancelTip");
		}
		else if (buttonRect.Contains(e.Location))
		{
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, "TroubleshootNextStageTip");
		}
		else if (launchRect.Contains(e.Location))
		{
			Cursor = Cursors.Hand;
			SlickTip.SetTo(this, "LaunchTooltip");
		}
		else
		{
			Cursor = Cursors.Default;
			SlickTip.SetTo(this, "TroubleshootBubbleTip");
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
		Padding = UI.Scale(new Padding(5), UI.FontScale);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!Live)
		{
			return;
		}

		e.Graphics.SetUp(BackColor);

		e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(75, FormDesign.Design.RedColor)), ClientRectangle, Padding.Left);

		var y = Padding.Top;

		using var buttonIcon = IconManager.GetSmallIcon("I_Skip");
		var buttonSize = SlickButton.GetSize(e.Graphics, buttonIcon, Locale.NextStage, UI.Font(6.75F), new(4, 3, 2, 2));
		buttonRect = ClientRectangle.Pad(Padding).Align(buttonSize, ContentAlignment.BottomRight);

		using var cancelButtonIcon = IconManager.GetSmallIcon("I_Cancel");
		buttonSize = SlickButton.GetSize(e.Graphics, cancelButtonIcon, LocaleSlickUI.Cancel, UI.Font(6.75F), new(4, 3, 2, 2));
		cancelRect = ClientRectangle.Pad(Padding).Align(buttonSize, ContentAlignment.BottomLeft);

		DrawProgress(e, ref y, buttonSize);

		e.Graphics.DrawStringItem(_troubleshootSystem.CurrentAction, Font, FormDesign.Design.MenuForeColor, Width - Padding.Horizontal, 0, ref y);

		y += Padding.Top;

		using var pen = new Pen(Color.FromArgb(150, FormDesign.Design.MenuForeColor), (float)UI.FontScale);
		e.Graphics.DrawLine(pen, Padding.Left, y, Width - Padding.Horizontal, y);

		y += Padding.Top;

		if (_troubleshootSystem.WaitingForGameLaunch || _troubleshootSystem.WaitingForGameClose && CrossIO.CurrentPlatform is Platform.Windows)
		{
			using var launchButtonIcon = IconManager.GetSmallIcon("I_CS");
			launchRect = new Rectangle(Padding.Left, y + buttonSize.Height + Padding.Bottom, Width - Padding.Horizontal, buttonSize.Height);

			SlickButton.DrawButton(e, launchRect, LocaleHelper.GetGlobalText(_citiesManager.IsRunning() ? "StopCities" : "StartCities"), UI.Font(6.75F), launchButtonIcon, null, launchRect.Contains(PointToClient(Cursor.Position)) ? HoverState & ~HoverState.Focused : HoverState.Normal, ColorStyle.Active);

			buttonRect.Y -= buttonSize.Height + Padding.Bottom;
			cancelRect.Y -= buttonSize.Height + Padding.Bottom;
			y += buttonSize.Height + Padding.Bottom;
		}

		SlickLabel.DrawLabel(e, buttonRect, Locale.NextStage, UI.Font(6.75F), buttonIcon, new Padding(4, 2, 2, 2), FormDesign.Design.MenuForeColor, buttonRect.Contains(PointToClient(Cursor.Position)) ? HoverState & ~HoverState.Focused : HoverState.Normal, ColorStyle.Green);
		SlickLabel.DrawLabel(e, cancelRect, LocaleSlickUI.Cancel, UI.Font(6.75F), cancelButtonIcon, new Padding(4, 2, 2, 2), FormDesign.Design.MenuForeColor, cancelRect.Contains(PointToClient(Cursor.Position)) ? HoverState & ~HoverState.Focused : HoverState.Normal, ColorStyle.Red);

		Height = y + buttonSize.Height + Padding.Bottom;
	}

	private void DrawProgress(PaintEventArgs e, ref int y, Size buttonSize)
	{
		var text = Locale.StageCounter.Format(_troubleshootSystem.CurrentStage, _troubleshootSystem.TotalStages);
		var barRect = new Rectangle(Padding.Left, y, Width - Padding.Horizontal, buttonSize.Height);
		using var backBarBrush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.MenuColor));
		using var activeBarBrush = Gradient(FormDesign.Design.ActiveColor, barRect, 1.5F);
		using var backTextBrush = new SolidBrush(FormDesign.Design.MenuForeColor);
		using var activeTextBrush = new SolidBrush(FormDesign.Design.ActiveForeColor);

		e.Graphics.FillRoundedRectangle(backBarBrush, barRect, (int)(4 * UI.FontScale));
		e.Graphics.DrawString(text, Font, backTextBrush, barRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

		e.Graphics.SetClip(new Rectangle(Padding.Left, 0, (Width - Padding.Horizontal) * _troubleshootSystem.CurrentStage / _troubleshootSystem.TotalStages, Height));

		e.Graphics.FillRoundedRectangle(activeBarBrush, barRect, (int)(4 * UI.FontScale));
		e.Graphics.DrawString(text, Font, activeTextBrush, barRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

		e.Graphics.ResetClip();

		y += barRect.Height + Padding.Bottom;
	}

	private async void AskForConfirmation()
	{
		if (MessagePrompt.Show(Locale.TroubleshootAskIfFixed, LocaleSlickUI.Confirmation, PromptButtons.YesNo, PromptIcons.Question, form: Program.MainForm) == DialogResult.Yes)
		{
			//if (_troubleshootSystem.CurrentStage * 2 >= _troubleshootSystem.TotalStages && MessagePrompt.Show(Locale.TroubleshootAskToStop, Locale.StopTroubleshootTitle, PromptButtons.YesNo, PromptIcons.Hand, form: Program.MainForm) == DialogResult.Yes)
			//{
			//	this.TryInvoke(Hide);
			//	await Task.Run(() => _troubleshootSystem.Stop(true));
			//	return;
			//}

			await Task.Run(() => _troubleshootSystem.ApplyConfirmation(true));
		}
		else
		{
			await Task.Run(() => _troubleshootSystem.ApplyConfirmation(false));
		}
	}

	private void PromptResult(List<ILocalPackage> list)
	{
		MessagePrompt.Show(Locale.TroubleshootCauseResult + "\r\n\r\n" + list.ListStrings("\r\n"), icon: PromptIcons.Ok, form: Program.MainForm);
	}

	protected override async void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.None || e.Button == MouseButtons.Left && buttonRect.Contains(e.Location))
		{
			if (_troubleshootSystem.WaitingForPrompt)
			{
				AskForConfirmation();
			}
			else if (_troubleshootSystem.WaitingForGameLaunch || _troubleshootSystem.WaitingForGameClose)
			{
				await Task.Run(_troubleshootSystem.NextStage);
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
			switch (MessagePrompt.Show(Locale.CancelTroubleshootMessage, Locale.CancelTroubleshootTitle, PromptButtons.YesNoCancel, PromptIcons.Hand, form: Program.MainForm))
			{
				case DialogResult.Yes:
					Hide();
					await Task.Run(() => _troubleshootSystem.Stop(false));
					break;
				case DialogResult.No:
					Hide();
					await Task.Run(() => _troubleshootSystem.Stop(true));
					break;
			}
		}
	}
}
