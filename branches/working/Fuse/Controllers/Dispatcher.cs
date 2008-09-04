using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Fuse.Controllers
{
	using Amplify.Linq;

	public class Dispatcher
	{
		private static List<Type> controllers;

		public void Test()
		{
			
		}

		static Dispatcher()
		{
			controllers = new List<Type>();
		}

		public static void LoadControllers(System.Reflection.Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			var x = types.Where(o => o.Name.EndsWith("Controller"));
			controllers.AddRange(x);
		}

		public static string Namespace
		{
			get;
			set;
		} 

		public static void Call(string uri)
		{
			int index = uri.IndexOf("?");
			string query = (index > -1) ? uri.Substring(uri.IndexOf("?")) : "";

			string[] parts = uri.Split("/");
			string controller = parts[0];
			string action = parts[1].Replace(query, "");


			Type type = controllers.SingleOrDefault(c => c.Name.ToLower().EndsWith(controller.ToLower() + "controller"));
			Controller obj = (Controller)Activator.CreateInstance(type);

			obj.Params.Clear();
			obj.Params.Add("controller", controller);
			obj.Params.Add("action", action);
			if (!string.IsNullOrEmpty(query))
			{
				query = query.Replace("?", "");
				string[] items = query.Split("&");
				foreach (string item in items)
				{
					parts = item.Split("=");
					obj.Params.Add(parts[0], parts[1]);
				}
			}

			System.Reflection.MethodInfo info = obj.GetType().GetMethod(action, System.Reflection.BindingFlags.IgnoreCase);
			info.Invoke(obj, null);
		}
	
	}
}
