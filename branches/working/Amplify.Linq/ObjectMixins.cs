

namespace Amplify.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.IO;
	using System.Reflection;
	using System.Text;
	using System.Runtime.Serialization.Json;
	using Amplify.Reflection;

	public static class ObjectMixins
	{

		public static Hash Hasherize(this object obj)
		{
			Hash h = new Hash();
			PropertyInfo[] properties = obj.GetType().GetProperties();
			foreach (PropertyInfo info in properties)
				if (info.CanRead)
					h.Add(info.Name, info.GetValue(obj, null));
			return h;
		}

		public static string ToJson(this object obj)
		{
			using(MemoryStream stream = new MemoryStream()) 
			{
				DataContractJsonSerializer serializer = new   DataContractJsonSerializer(obj.GetType());
				serializer.WriteObject(stream, obj);
				stream.Position = 0;
				return new StreamReader(stream).ReadToEnd();
			}
		}

		public static T Default<T>(this object obj, T value)
		{
			return (obj != null) ? (T)obj : value;
		}
	}
}
