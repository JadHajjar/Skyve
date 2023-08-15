using SkyveApp.Domain.Systems;

using System.Collections.Generic;

namespace SkyveApp.Domain;

public delegate void MonitorTickDelegate(bool isAvailable, bool isRunning);

public delegate void PromptMissingItemsDelegate(IPlaysetManager manager, IEnumerable<IPlaysetEntry> playsetEntries);