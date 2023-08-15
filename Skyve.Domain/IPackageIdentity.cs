﻿namespace SkyveApp.Domain;

public interface IPackageIdentity
{
	ulong Id { get; }
	string Name { get; }
	string? Url { get; }
}