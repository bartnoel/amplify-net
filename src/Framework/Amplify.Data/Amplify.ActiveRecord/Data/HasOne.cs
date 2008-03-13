

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Collections;
	using System.Linq;
	using System.Text;
	using System.Reflection;

	using Amplify.Linq;

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)] 
	public class HasOne : AssociationAttribute 
	{
		private string findersql = "";
		
		public AssociationDependency Dependent { get; set; }
		public string As { get; set; }
		
		public string Order { get; set; }
		public string Conditions { get; set; }
		public IDictionary<string, object> Includes { get; set; }


		public string FinderSql
		{
			get {
				if (string.IsNullOrEmpty(this.findersql))
					this.findersql = this.GetFinderSql();
				return this.findersql;
			}
			set {
				if (!Object.Equals(this.findersql, value))
					this.findersql = value;
			}
		}

		private string GetFinderSql()
		{
			object item = Activator.CreateInstance(this.ClassType);
			//if(
		}

		public override object TypeId
		{
			get
			{
				return this.AssociationId;
			}
		}

		
	}
}
