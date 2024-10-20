using Microsoft.Extensions.DependencyInjection;

using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility;

using System.Windows.Forms;

namespace Skyve.Systems;
public static class SystemsProgram
{
	public static Form? MainForm { get; set; }

	public static IServiceCollection AddSkyveSystems(this IServiceCollection services)
	{
		services.AddSingleton<ILocale, Locale>();
		services.AddSingleton<IImageService, ImageService>();
		services.AddSingleton<IIOUtil, IOUtil>();
		services.AddSingleton<INotifier, NotifierSystem>();
		services.AddSingleton<IPackageNameUtil, PackageNameUtil>();
		services.AddSingleton<IPackageUtil, PackageUtil>();
		services.AddSingleton<ICompatibilityManager, CompatibilityManager>();

		services.AddTransient<ILoadOrderHelper, LoadOrderHelper>();
		services.AddTransient<ApiUtil>();

		return services;
	}
}
