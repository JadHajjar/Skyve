﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Domain;
public interface IBackupSettings
{
	string? DestinationFolder { get; set; }
}
