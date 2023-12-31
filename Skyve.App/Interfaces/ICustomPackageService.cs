﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.App.Interfaces;
public interface ICustomPackageService
{
	SlickStripItem[] GetRightClickMenuItems(IEnumerable<IPackage> packages);
	SlickStripItem[] GetRightClickMenuItems(IPackage package);
}
