using System;
using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface IDlcManager
{
	event Action DlcsLoaded;

	IEnumerable<IDlcInfo> Dlcs { get; }

	bool IsAvailable(uint dlcId);
	void SetExcludedDlcs(IEnumerable<uint> uints);
	bool IsIncluded(IDlcInfo dlc);
	void SetIncluded(IDlcInfo dlc, bool value);
	List<uint> GetExcludedDlcs();
}
