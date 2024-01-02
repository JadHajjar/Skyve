using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Domain;
public class PackageInstallProgress
{
	public ulong Id { get; set; }
	public float Progress { get; set; }
}
