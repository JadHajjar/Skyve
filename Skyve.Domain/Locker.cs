using System;

namespace Skyve.Domain;

public class Locker : IDisposable
{
	public bool Locked { get; set; }

	public void Dispose()
	{
		Locked = false;
	}
}