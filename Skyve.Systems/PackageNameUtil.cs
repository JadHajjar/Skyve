using Extensions;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Skyve.Systems;

public class PackageNameUtil : IPackageNameUtil
{
	private readonly ILocale _locale;
	private readonly Regex _tagRegex = new(@"v?\d+\.\d+(\.\d+)*(-[\d\w]+)*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
	private readonly Regex _bracketsRegex = new(@"[\[\(](.+?)[\]\)]", RegexOptions.Compiled);

	public PackageNameUtil(ILocale locale)
	{
		_locale = locale;
	}

	public string CleanName(IPackageIdentity? package, bool keepTags = false)
	{
		if (package?.Name is null)
		{
			return string.Empty;
		}

		var text = _tagRegex.Replace(package.Name, string.Empty);

#if CS1
		if (package is IPackage lp && lp.IsBuiltIn)
		{
			text = text.FormatWords();
		}
#endif
		return keepTags
			? text.RemoveDoubleSpaces().RegexRemove(" +(?=[\\]\\)])").RegexRemove("(?<=[\\[\\(]) +")
			: _bracketsRegex.Replace(text, string.Empty).Trim('-', ']', '[', '(', ')', ' ').RemoveDoubleSpaces();
	}

	public string CleanName(IPackageIdentity? package, out List<(Color Color, string Text)> tags, bool keepTags = false)
	{
		tags = [];

		if (package?.Name is null or "")
		{
			return _locale.Get("UnknownPackage");
		}

		if (package is IAsset)
		{
			return package.Name;
		}

		var isLocal = package.IsLocal();
		var text = _tagRegex.Replace(package.Name, string.Empty);
		var tagMatches = _bracketsRegex.Matches(text);

		text = keepTags
			? text.RemoveDoubleSpaces().RegexRemove(" +(?=[\\]\\)])").RegexRemove("(?<=[\\[\\(]) +")
			: _bracketsRegex.Replace(text, string.Empty).Trim('-', ']', '[', '(', ')', ' ').RemoveDoubleSpaces();

#if CS1
		if (lp?.IsBuiltIn ?? false)
		{
			text = text.FormatWords();
		}
		else
#endif
		if (isLocal)
		{
			tags.Add((FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.AccentColor).MergeColor(FormDesign.Design.BackColor, 65), _locale.Get("Local").One.ToUpper()));
		}

		foreach (Match match in tagMatches)
		{
			var tagText = match.Groups[1].Value.Trim();

			if (!tags.Any(x => x.Text.Equals(tagText, StringComparison.InvariantCultureIgnoreCase)))
			{
				if (tagText.ToLower() is "stable" or "deprecated" or "obsolete" or "abandoned" or "broken")
				{
					continue;
				}

				var color = tagText.ToLower() switch
				{
					"alpha" or "experimental" => Color.FromArgb(200, FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.RedColor)),
					"beta" or "test" or "testing" => Color.FromArgb(180, FormDesign.Design.YellowColor),
					_ => (Color?)null
				};

				if (isLocal && color is not null)
				{
					continue;
				}

				tags.Add((color ?? FormDesign.Design.ButtonColor, color is null ? tagText : LocaleHelper.GetGlobalText(tagText).One.ToUpper()));
			}
		}

		var workshopInfo = package.GetWorkshopInfo();

		if (workshopInfo is null)
		{
			return text;
		}

		if (workshopInfo.IsBanned)
		{
			tags.Add((FormDesign.Design.RedColor, _locale.Get("Banned").One.ToUpper()));
		}
		else if (workshopInfo.IsIncompatible)
		{
			tags.Add((FormDesign.Design.RedColor, _locale.Get("Incompatible").One.ToUpper()));
		}
		else
		{
			var info = package.GetPackageInfo();

			if (info?.Stability is PackageStability.Broken)
			{
				tags.Add((Color.FromArgb(225, FormDesign.Design.RedColor), _locale.Get("Broken").One.ToUpper()));
			}
		}

		return text;
	}

	public string GetVersionText(string name)
	{
		var match = Regex.Match(name, @"v?(\d+\.\d+(\.\d+)*(-[\d\w]+)*)", RegexOptions.IgnoreCase);

		return match.Success ? "v" + match.Groups[1].Value : string.Empty;
	}
}