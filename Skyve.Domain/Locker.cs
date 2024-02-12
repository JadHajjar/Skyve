using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Domain;

public class Locker : IDisposable
{
	public bool Locked { get; set; }

	public void Dispose()
	{
		Locked = false;
	}
}