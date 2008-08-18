using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public class GuidFactory : DefaultValueFactory 
	{

		public GuidFactory()
		{
			this.IsEmpty = false;
		}

		public GuidFactory(bool isEmpty)
		{
			this.IsEmpty = isEmpty;
		}

		public bool IsEmpty { get; set; }

		public override object DefaultValue
		{
			get { 
				if(this.IsEmpty)
					return Guid.Empty;
				return Guid.NewGuid();
			}
		}

		public override object CreateDefaultValue(IPropertyInfo info)
		{
			return this.DefaultValue;
		}

		
	}
}
