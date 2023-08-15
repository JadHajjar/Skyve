using Microsoft.Extensions.DependencyInjection;

using SkyveApp.Domain.Systems;
using SkyveApp.Systems.Compatibility;

using System.Windows.Forms;

namespace SkyveApp.Systems;
public static class SystemsProgram
{
	public static Form? MainForm { get; set; }

	public static IServiceCollection AddSkyveSystems(this IServiceCollection services)
	{
		services.AddTransient<IBulkUtil, BulkUtil>();
		services.AddSingleton<IImageService, ImageSystem>();
		services.AddSingleton<IIOUtil, IOUtil>();
		services.AddSingleton<ILogger, LoggerSystem>();
		services.AddSingleton<INotifier, NotifierSystem>();
		services.AddTransient<IPackageNameUtil, PackageNameUtil>();
		services.AddTransient<IPackageUtil, PackageUtil>();
		services.AddTransient<SkyveApiUtil>();
		services.AddTransient<ILoadOrderHelper, LoadOrderHelper>();
		services.AddSingleton<ICompatibilityManager, CompatibilityManager>();

		return services;
	}
}
