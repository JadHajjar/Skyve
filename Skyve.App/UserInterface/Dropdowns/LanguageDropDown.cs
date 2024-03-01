using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class LanguageDropDown : SlickSelectionDropDown<string>
{
	#region Names
	private static readonly Dictionary<string, Dictionary<bool, (string Name, string Dialect)>> _langNames = new()
	{
		["en-US"] = new()
		{
			[false] = ("English", "United States"),
			[true] = ("English", "United States")
		},
		["ar-SA"] = new()
		{
			[false] = ("Arabic", "Saudi Arabia"),
			[true] = ("العربية", "المملكة العربية السعودية")
		},
		["bn-BD"] = new()
		{
			[false] = ("Bangla", "Bangladesh"),
			[true] = ("বাংলা", "বাংলাদেশ")
		},
		["zh-CN"] = new()
		{
			[false] = ("Chinese", "Simplified, China"),
			[true] = ("中文", "中国")
		},
		["zh-TW"] = new()
		{
			[false] = ("Chinese", "Traditional, Taiwan"),
			[true] = ("中文", "台灣")
		},
		["cs-CZ"] = new()
		{
			[false] = ("Czech", "Czechia"),
			[true] = ("Čeština", "Česko")
		},
		["nl-NL"] = new()
		{
			[false] = ("Dutch", "Netherlands"),
			[true] = ("Nederlands", "Nederland")
		},
		["en-GB"] = new()
		{
			[false] = ("English", "United Kingdom"),
			[true] = ("English", "United Kingdom")
		},
		["fi-FI"] = new()
		{
			[false] = ("Finnish", "Finland"),
			[true] = ("Suomi", "Suomi")
		},
		["fr-FR"] = new()
		{
			[false] = ("French", "France"),
			[true] = ("Français", "France")
		},
		["ka-GE"] = new()
		{
			[false] = ("Georgian", "Georgia"),
			[true] = ("ქართული", "საქართველო")
		},
		["de-DE"] = new()
		{
			[false] = ("German", "Germany"),
			[true] = ("Deutsch", "Deutschland")
		},
		["hi-IN"] = new()
		{
			[false] = ("Hindi", "India"),
			[true] = ("हिन्दी", "भारत")
		},
		["id-ID"] = new()
		{
			[false] = ("Indonesian", "Indonesia"),
			[true] = ("Indonesia", "Indonesia")
		},
		["it-IT"] = new()
		{
			[false] = ("Italian", "Italy"),
			[true] = ("Italiano", "Italia")
		},
		["ja-JP"] = new()
		{
			[false] = ("Japanese", "JapanJapan"),
			[true] = ("日本語", "日本")
		},
		["ko-KR"] = new()
		{
			[false] = ("Korean", "Korea"),
			[true] = ("한국어", "대한민국")
		},
		["pl-PL"] = new()
		{
			[false] = ("Polish", "Poland"),
			[true] = ("Polski", "Polska")
		},
		["pt-BR"] = new()
		{
			[false] = ("Portuguese", "Brazil"),
			[true] = ("Português", "Brasil")
		},
		["pt-PT"] = new()
		{
			[false] = ("Portuguese", "Portugal"),
			[true] = ("Português", "Portugal")
		},
		["ro-RO"] = new()
		{
			[false] = ("Romanian", "Romania"),
			[true] = ("Română", "România")
		},
		["ru-RU"] = new()
		{
			[false] = ("Russian", "Russia"),
			[true] = ("Русский", "Россия")
		},
		["es-ES"] = new()
		{
			[false] = ("Spanish", "Spain"),
			[true] = ("Español", "España")
		},
		["sv-SE"] = new()
		{
			[false] = ("Swedish", "Sweden"),
			[true] = ("Svenska", "Sverige")
		},
		["th-TH"] = new()
		{
			[false] = ("Thai", "Thailand"),
			[true] = ("ไทย", "ไทย")
		},
		["tr-TR"] = new()
		{
			[false] = ("Turkish", "Turkey"),
			[true] = ("Türkçe", "Türkiye")
		},
		["uk-UA"] = new()
		{
			[false] = ("Ukrainian", "Ukraine"),
			[true] = ("Українська", "Україна")
		},
		["no-NO"] = new()
		{
			[false] = ("Norwegian", "Norway"),
			[true] = ("Norsk", "Noreg")
		},
		["vi-VN"] = new()
		{
			[false] = ("Vietnamese", "Vietnam"),
			[true] = ("Tiếng Việt", "Việt Nam")
		},
		["hu-HU"] = new()
		{
			[false] = ("Hungarian", "Hungary"),
			[true] = ("Magyar", "Magyarország")
		},
		["sr-CS"] = new()
		{
			[false] = ("Serbian", "Serbia"),
			[true] = ("Srpski", "Србија")
		},
		["el-GR"] = new()
		{
			[false] = ("Greek", "Greece"),
			[true] = ("Ελληνικά", "Ελλάδα")
		},
	};
	#endregion

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (Live)
		{
			ItemHeight = 22;
		}
	}

	protected override void UIChanged()
	{
		Font = UI.Font("Segoe UI", 8.25F);
		Padding = UI.Scale(new Padding(5), UI.FontScale);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);

		Height = (int)(42 * UI.UIScale);
	}

	protected override IEnumerable<string> OrderItems(IEnumerable<string> items)
	{
		return items.OrderByDescending(x => x == "en-US").ThenBy(x => _langNames[x][false].Name);
	}

	protected override bool SearchMatch(string searchText, string item)
	{
		var names = new[]
		{
			_langNames[item][false].Name,
			_langNames[item][false].Dialect,
			_langNames[item][true].Name,
			_langNames[item][true].Dialect,
		};

		return names.Any(x => searchText.SearchCheck(x));
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, string item)
	{
		if (item == null)
		{
			return;
		}

		using var icon = (Bitmap)Properties.Resources.ResourceManager.GetObject("Lang_" + item.ToUpper(), Properties.Resources.Culture);

		if (icon != null)
		{
			var iconSize = Math.Min(48, (int)(16 * UI.UIScale));

			e.Graphics.DrawImage(icon, rectangle.Pad(Padding).Align(new Size(iconSize, iconSize), ContentAlignment.MiddleLeft));

			var text = _langNames[item][hoverState.HasFlag(HoverState.Hovered)];
			var textSize1 = Size.Ceiling(e.Graphics.Measure(text.Name, Font));
			using var font = UI.Font(7F);
			var textSize2 = Size.Ceiling(e.Graphics.Measure(" / " + text.Dialect, font));
			var textRect = rectangle.Pad(iconSize + Padding.Horizontal, (int)(ItemHeight * UI.FontScale) + 1 != rectangle.Height ? -1 : -Padding.Top + 1, Padding.Right * 3 / 2, (int)(ItemHeight * UI.FontScale) + 1 != rectangle.Height ? -3 : -Padding.Bottom + 1);
			var textRect1 = textRect.Align(new Size(textRect.Width, textSize1.Height), textSize1.Width + textSize2.Width > textRect.Width ? ContentAlignment.TopCenter : ContentAlignment.MiddleCenter);
			var textRect2 = textRect.Align(new Size(textRect.Width, textSize2.Height), textSize1.Width + textSize2.Width > textRect.Width ? ContentAlignment.BottomCenter : ContentAlignment.MiddleCenter);

			textRect.Width = rectangle.Width - textRect.X;

			using var brush = new SolidBrush(foreColor);
			using var format = new StringFormat { Trimming = StringTrimming.EllipsisCharacter };
			e.Graphics.DrawString(text.Name, base.Font, brush, textRect1, format);

			using var brush1 = new SolidBrush(Color.FromArgb(175, foreColor));
			using var format1 = new StringFormat { Alignment = StringAlignment.Far, Trimming = StringTrimming.EllipsisCharacter };
			e.Graphics.DrawString(" / " + text.Dialect, font, brush1, textRect2, format1);
		}
	}
}
