using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuse.Models
{
	internal interface IFill
	{
		void Fill(System.Data.IDataReader datareader, IDictionary<string,object> includes);
		bool IsDeferred { get; set; }
		void Load(string name, IFill parent);
		void SetParent(string name, IFill parent);
		void RemoveParent(string name);
		object this[string propertyName] { get; set; }

	}
}
