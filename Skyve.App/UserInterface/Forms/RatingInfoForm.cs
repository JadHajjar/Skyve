using SkyveApp.Systems.CS1.Utilities;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Forms;
public partial class RatingInfoForm : SlickForm
{
	public RatingInfoForm()
	{
		InitializeComponent();

		L_Title.Text = Locale.VotingTitle;
		L_1.Text = Locale.VotingInfo1;
		L_2.Text = Locale.VotingInfo2 + "\r\n" + Locale.VotingInfo3 + "\r\n" + string.Format(Locale.VotingInfo4, "1/10");
		L_3.Text = string.Format(Locale.VotingInfo5, 15000.ToString("N0"));
		L_4.Text = string.Format(Locale.VotingInfo6, 50000.ToString("N0"));
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_Main.BackColor = design.BackColor;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		slickIcon1.Size = UI.Scale(new Size(72, 72), UI.UIScale);
		TLP_Main.Padding = slickIcon1.Padding = UI.Scale(new Padding(16), UI.UIScale);
		L_Title.Font = UI.Font(14F, FontStyle.Bold);
		B_Ok.Font = UI.Font(9.75F);
		L_1.Font = L_2.Font = L_3.Font = L_4.Font = UI.Font(9F);
		L_1.Margin = L_2.Margin = L_3.Margin = L_4.Margin = UI.Scale(new Padding(10), UI.UIScale);
		PB_11.Size = PB_12.Size = PB_13.Size = PB_3.Size = PB_4.Size = UI.Scale(new Size(24, 24), UI.UIScale);
	}

	private void B_Ok_Click(object sender, EventArgs e)
	{
		Close();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			Close();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void PB_1_Paint(object sender, PaintEventArgs e)
	{
		var subscriptions = 0;
		var score = 35;
		Draw(e, subscriptions, score);
	}

	private void PB_12_Paint(object sender, PaintEventArgs e)
	{
		var subscriptions = 200;
		var score = 60;
		Draw(e, subscriptions, score);
	}

	private void PB_13_Paint(object sender, PaintEventArgs e)
	{
		var subscriptions = 1000;
		var score = 85;
		Draw(e, subscriptions, score);
	}

	private void PB_3_Paint(object sender, PaintEventArgs e)
	{
		var subscriptions = 10000;
		var score = 100;
		Draw(e, subscriptions, score);
	}

	private void PB_4_Paint(object sender, PaintEventArgs e)
	{
		var subscriptions = 50000;
		var score = 100;
		Draw(e, subscriptions, score);
	}

	private void Draw(PaintEventArgs e, int subscriptions, int score)
	{
		e.Graphics.SetUp(PB_11.BackColor);
		var clip = e.Graphics.ClipBounds;
		var labelH = PB_11.Height - (2 * (int)(2 * UI.FontScale));
		labelH -= labelH % 2;
		var small = UI.FontScale < 1.25;
		var scoreRect = PB_11.ClientRectangle.CenterR(labelH, labelH);
		var backColor = score > 90 && subscriptions >= 50000 ? FormDesign.Modern.ActiveColor : FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.RedColor, score).MergeColor(FormDesign.Design.BackColor, 75);

		if (!small)
		{
			e.Graphics.FillEllipse(new SolidBrush(backColor), scoreRect);
		}
		else
		{
			scoreRect.Y--;
		}
		var scoreFilled = IconManager.GetSmallIcon("I_VoteFilled");

		if (score < 75)
		{
			using var scoreIcon = IconManager.GetSmallIcon("I_Vote");

			e.Graphics.DrawImage(scoreIcon.Color(small ? backColor : backColor.GetTextColor()), scoreRect.CenterR(scoreIcon.Size));

			e.Graphics.SetClip(scoreRect.CenterR(scoreFilled.Size).Pad(0, scoreFilled.Height - (scoreFilled.Height * score / 105), 0, 0));
			e.Graphics.DrawImage(scoreFilled.Color(small ? backColor : backColor.GetTextColor()), scoreRect.CenterR(scoreFilled.Size));
			e.Graphics.SetClip(clip);
		}
		else
		{
			e.Graphics.DrawImage(scoreFilled.Color(small ? backColor : backColor.GetTextColor()), scoreRect.CenterR(scoreFilled.Size));
		}

		if (subscriptions < 50000 || score <= 90)
		{
			if (small)
			{
				using var scoreIcon = IconManager.GetSmallIcon("I_Vote");

				e.Graphics.SetClip(scoreRect.CenterR(scoreIcon.Size).Pad(0, scoreIcon.Height - (scoreIcon.Height * subscriptions / 15000), 0, 0));
				e.Graphics.DrawImage(scoreIcon.Color(FormDesign.Modern.ActiveColor), scoreRect.CenterR(scoreIcon.Size));
				e.Graphics.SetClip(clip);
			}
			else
			{
				using var pen = new Pen(Color.FromArgb(score >= 75 ? 255 : 200, FormDesign.Modern.ActiveColor), (float)(1.5 * UI.FontScale)) { EndCap = LineCap.Round, StartCap = LineCap.Round };
				e.Graphics.DrawArc(pen, scoreRect.Pad(-1), 90 - (Math.Min(360, 360F * subscriptions / 15000) / 2), Math.Min(360, 360F * subscriptions / 15000));
			}
		}
	}
}
