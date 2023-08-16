using System.Reflection;

namespace Skyve.App.UserInterface.Panels;
internal class PC_LotChangeLog : PC_Changelog
{
	public PC_LotChangeLog() : base(
		Assembly.GetExecutingAssembly(),
		$"Skyve.App.CS1.Changelog.json",
		Assembly.GetExecutingAssembly().GetName().Version)
	{
		LocaleChangelog.Load();
	}

	protected override void PrepareChangelog(List<VersionChangeLog> changeLogs)
	{
#if !DEBUG
#if Stable
		changeLogs.RemoveAll(x => x.Beta);
#else
		changeLogs.RemoveAll(x => x.Stable);
#endif
		if (System.Diagnostics.Debugger.IsAttached)
		{
			var current = changeLogs.First();

            System.Windows.Forms.Clipboard.SetText($"# :skyve: Skyve v{current.VersionString}{(current.Stable ? " [Stable]" : "")}{(current.Beta ? " [Beta]" : "")}\r\n"
				+ (string.IsNullOrEmpty(current.Tagline) ? string.Empty : $"### *{current.Tagline}*\r\n")
				+ current.ChangeGroups.ListStrings(x => $"## {x.Name}\r\n{x.Changes.ListStrings(y => $"* {y}", "\r\n")}", "\r\n\r\n"));
		}
#else
		if (System.Diagnostics.Debugger.IsAttached)
		{
			var texts = new List<string>();

			foreach (var changelog in changeLogs)
			{
				texts.Add(changelog.Tagline);
				texts.AddRange(changelog.ChangeGroups.Select(x => x.Name));
				texts.AddRange(changelog.ChangeGroups.SelectMany(x => x.Changes));
			}

			var json = Newtonsoft.Json.JsonConvert.SerializeObject(texts.WhereNotEmpty().Distinct().OrderBy(x => x.Length).ToDictionary(x => x), Newtonsoft.Json.Formatting.Indented);

			System.IO.File.WriteAllText("../../../Properties/Changelog.json", json);
		}
#endif
	}
}
