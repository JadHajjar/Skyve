using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;

public class UserIcon : SlickImageControl
{
	private readonly IWorkshopService _workshopService;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IUser? User { get; set; }
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool Collection { get; set; }

	public UserIcon()
	{
		ServiceCenter.Get(out _workshopService);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var author = _workshopService?.GetUser(User);
		var thumbnail = author?.GetThumbnail();

		Loading = thumbnail is null && author?.AvatarUrl is not null and not "" && ConnectionHandler.IsConnected;

		if (Loading)
		{
			using var accentBrush = new SolidBrush(FormDesign.Design.AccentBackColor);
			e.Graphics.FillRoundedRectangle(accentBrush, ClientRectangle.Pad(1), (int)(5 * UI.FontScale));

			DrawLoader(e.Graphics, ClientRectangle.CenterR(UI.Scale(new Size(32, 32), UI.UIScale)));
			return;
		}

		e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

		if (thumbnail != null)
		{
			e.Graphics.DrawRoundedImage(thumbnail, ClientRectangle.Pad(1), (int)(5 * UI.FontScale), FormDesign.Design.AccentBackColor);
			return;
		}

		using var brush = new SolidBrush(GetUserColor(User?.Id?.ToString() ?? string.Empty));
		using var generic = IconManager.GetIcon("User", Height * 8 / 10).Color(brush.Color.GetTextColor());

		e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1), (int)(5 * UI.FontScale));
		e.Graphics.DrawImage(generic, ClientRectangle.CenterR(generic.Size));
	}

	public static Color GetUserColor(string username, bool textColor = false)
	{
		if (!ServiceCenter.Get<ISettings>()?.UserSettings.ColoredAuthorNames ?? false)
		{
			return FormDesign.Design.ForeColor;
		}

		// Compute a hash from the input string
		var hash = username.GetHashCode();

		// Use the hash to generate RGB values
		// We'll use the lower 24 bits of the hash for the color
		var r = (hash & 0xFF0000) >> 16;
		var g = (hash & 0x00FF00) >> 8;
		var b = hash & 0x0000FF;

		// Create the color from the RGB values
		var color = Color.FromArgb(r, g, b);

		if (!textColor)
		{
			return color;
		}

		// adjust for better text readability 
		return color.MergeColor(FormDesign.Design.ForeColor, 75).Tint(Lum: FormDesign.Design.IsDarkTheme ? 4 : -2.5f, Sat: 3);
	}
}
