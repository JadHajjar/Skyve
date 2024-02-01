using Extensions;

using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Skyve.Systems.Compatibility;
internal class CompatibilityService
{
	private readonly ILocale _locale;
	private readonly ILogger _logger;
	private readonly IPackageManager _contentManager;
	private readonly ICompatibilityUtil _compatibilityUtil;
	private readonly IPackageUtil _contentUtil;
	private readonly IPackageNameUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly IDlcManager _dlcManager;
	private readonly CompatibilityHelper _compatibilityHelper;
	private readonly CompatibilityManager _compatibilityManager;

	public CompatibilityService(ILocale locale, ILogger logger, IPackageManager contentManager, ICompatibilityUtil compatibilityUtil, IPackageUtil contentUtil, IPackageNameUtil packageUtil, IWorkshopService workshopService, IDlcManager dlcManager, CompatibilityHelper compatibilityHelper, CompatibilityManager compatibilityManager)
	{
		_locale = locale;
		_logger = logger;
		_contentManager = contentManager;
		_compatibilityUtil = compatibilityUtil;
		_contentUtil = contentUtil;
		_packageUtil = packageUtil;
		_workshopService = workshopService;
		_dlcManager = dlcManager;
		_compatibilityHelper = compatibilityHelper;
		_compatibilityManager = compatibilityManager;
	}

}
