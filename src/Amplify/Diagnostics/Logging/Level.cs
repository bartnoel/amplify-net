﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify.Diagnostics.Logging
{
	public enum Level
	{
		Fatal = 1000,
		Exception = 2000,
		Warn = 2000,
		Sql = 4000,
		Info = 5000,
		Debug = 6000
	}
}
