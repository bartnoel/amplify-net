using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] 
	public class BelongsTo : AssociationAttribute 
	{

		public bool CounterCache { get; set; }
		//TODO create counter cache delegate
		public bool Polymorphic { get; set; }
	}
}
