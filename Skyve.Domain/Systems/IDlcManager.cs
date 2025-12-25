using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IDlcManager
{
	event Action DlcsLoaded;

	IEnumerable<IDlcInfo> Dlcs { get; }

	bool IsAvailable(ulong dlc);
	bool IsAvailable(IDlcInfo dlc);
	void SetExcludedDlcs(IEnumerable<IDlcInfo> uints);
	bool IsIncluded(IDlcInfo dlc);
	void SetIncluded(IDlcInfo dlc, bool value);
	List<IDlcInfo> GetExcludedDlcs();
	IDlcInfo? TryGetDlc(string displayName, bool exact = false);
	IDlcInfo? TryGetDlc(ulong dlc, bool exact = false);
	Task UpdateDLCs();
}
