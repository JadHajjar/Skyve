using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssemblyResolver
{
	internal class Program
	{
		private static string ManagedDLL;
		private static string ModsPath;
		private static string WorkshopContentPath;

		static void Main(string[] args)
		{
			ManagedDLL = args[1];
			ModsPath = args[2];
			WorkshopContentPath = args[3];
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;

			var assembly = Assembly.ReflectionOnlyLoadFrom(args[0]);
			var types = assembly.GetExportedTypes();

			foreach (var type in types)
			{
				var interfaces = type.GetInterfaces();

				foreach (var iface in interfaces)
				{
					if (iface.FullName == "ICities.IUserMod")
					{
						Environment.Exit(1);

						return;
					}
				}
			}

			Environment.Exit(2);
		}

		private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
		{
			var searchFolders = new string[]
			{
				ManagedDLL,
				Path.GetDirectoryName(args.RequestingAssembly.Location),
				ModsPath,
				WorkshopContentPath
			};

			var assemblyName = new AssemblyName(args.Name).Name + ".dll";

			foreach (var folder in searchFolders)
			{
				var assembly = Directory.GetFiles(folder, assemblyName, SearchOption.AllDirectories).FirstOrDefault();

				return Assembly.ReflectionOnlyLoadFrom(assembly);
			}

			return null;
		}
	}
}
