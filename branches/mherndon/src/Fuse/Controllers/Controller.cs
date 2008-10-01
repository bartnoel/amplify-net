using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fuse.Controllers
{
	using Amplify.Linq;

	public class Controller
	{
		private NameValueCollection parameters = new NameValueCollection();

		public string Namespace { get; set; }


		public NameValueCollection Params
		{
			get
			{
				return this.parameters;
			}
		}

		public enum RenderType {
			Window, 
			Page,
			Http,
			Partial
		}

		static Controller()
		{

		}

		public Controller()
		{
			this.Namespace = "Fuse.Views.";
		}

		public void Render(Hash options) {
			string uri = (options["uri"] as string);
			object data = (options["locals"] as string);
			if (!string.IsNullOrEmpty(uri))
			{
				if (uri.StartsWith("http") || uri.StartsWith("www."))
					this.Render(RenderType.Http, uri);
			}

			string window = (options["window"] as string);
			if (!string.IsNullOrEmpty(window))
			{
				string partial = (options["partial"] as string);
				this.Render(RenderType.Window, window, data, partial);
			}
		}

		public void Render(RenderType type, string uri)
		{
			this.Render(type, uri, null, null);
		}

		public void Render(RenderType type, string uri, object locals, string partial)
		{
			switch (type)
			{
				case RenderType.Window:
					Type windowType = Type.GetType(this.Namespace + uri.Replace("/", "."));
					
					if(windowType == null)
						throw new InvalidCastException("Could not find type: " + this.Namespace + uri.Replace("/", "."));
					
					Window window = (Window)Activator.CreateInstance(windowType);
					
					if (locals != null || !string.IsNullOrEmpty(partial)) 
					{
						Hash data = (locals is Hash) ? (Hash)locals : locals.Hasherize();
						if(!string.IsNullOrEmpty(partial))
							if(data == null)
								data = Hash.New(Partial => partial);
							else 
								data["Partial"] = partial;

						this.SetProperties(window, data);
					}
					window.Show();
					break;
				case RenderType.Page:
					break;
				case RenderType.Http:
					System.Diagnostics.Process.Start(uri);
					break;
			}
		}

		

		protected void SetProperties(object container, Hash locals)
		{
			System.Reflection.PropertyInfo[] properties = container.GetType().GetProperties();
			foreach (KeyValuePair<string, object> pair in locals)
			{
				System.Reflection.PropertyInfo property =
					properties.SingleOrDefault(p => p.Name == pair.Key);
				if(property != null && property.CanWrite)
					property.SetValue(container, pair.Value, null);
					
			}
		}


	}
}
