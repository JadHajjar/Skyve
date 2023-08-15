using Microsoft.Extensions.DependencyInjection;

using SkyveApp.Systems.CS1;
using SkyveApp.Systems.CS1.Utilities;

using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using static System.Environment;

namespace SkyveApp;
#nullable disable
public static class Program
{
	public static bool IsRunning { get; set; }
	public static string CurrentDirectory { get; set; }
	public static string ExecutablePath { get; set; }
	public static MainForm MainForm { get; set; }
}
#nullable enable
