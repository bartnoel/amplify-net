﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.ActiveRecord
{
	public interface IParent
	{
		void RemoveChild(object child);
	}
}