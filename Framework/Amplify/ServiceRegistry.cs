//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class ServiceRegistry : IServiceProvider 
	{
		private Dictionary<string, object> services;

		public ServiceRegistry()
		{
			this.services = new Dictionary<string, object>();
		}


		public object this[string name]
		{
			get { return this.services[name]; }
		}

		public bool Contains(string name)
		{
			return this.services.ContainsKey(name);
		}

		public bool Contains(IService service)
		{
			return this.services.ContainsKey(service.GetType().ToString());
		}

		public bool Contains(Type service)
		{
			return this.services.ContainsKey(service.ToString());
		}

		public void Add<T>(T service) where T: IService 
		{
			this.services.Add(typeof(T).ToString(), service);
		}

		public void Add(IService service)
		{
			this.services.Add(service.GetType().ToString(), service);
		}

		public void Add(string name, IService service)
		{
			this.services.Add(name, service);
		}

		public T GetService<T>()
		{
			Type get = typeof(T);
			if (!this.services.ContainsKey(get.ToString()))
				this.services.Add(get.ToString(), Activator.CreateInstance<T>());

			return (T)this.services[get.ToString()];
		}

		public object GetService(Type serviceType)
		{
			if (!this.services.ContainsKey(serviceType.ToString()))
				this.services.Add(serviceType.ToString(), Activator.CreateInstance(serviceType));

			return this.services[serviceType.ToString()];
		}

		public object GetService(string name)
		{
			if (!this.services.ContainsKey(name))
				return null;
			return this.services[name];
		}

		public bool Remove(string name)
		{
			return this.services.Remove(name);
		}

		public bool Remove<T>() where T: IService 
		{
			if(this.services.ContainsKey(typeof(T).ToString()))
				return this.services.Remove(typeof(T).ToString());
			return false;
		}

		public bool Remove(IService service)
		{
			Type type = service.GetType();
			if (this.services.ContainsKey(type.ToString()))
				return this.services.Remove(type.ToString());
			return false;
		}

		public bool Remove(Type serviceType)
		{
			if(this.services.ContainsKey(serviceType.ToString()))
				return this.services.Remove(serviceType.ToString());
			return false;
		}
	}
}
