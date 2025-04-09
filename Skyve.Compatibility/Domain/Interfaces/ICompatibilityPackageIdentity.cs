using Skyve.Domain;

namespace Skyve.Compatibility.Domain.Interfaces;

public interface ICompatibilityPackageIdentity : IPackageIdentity
{
    bool IsDlc { get; }
}
