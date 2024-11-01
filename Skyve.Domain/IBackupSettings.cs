using Extensions;

namespace Skyve.Domain;
public interface IBackupSettings : IExtendedSaveObject
{
	string? DestinationFolder { get; set; }
}
