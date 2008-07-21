//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ComponentModel
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// A lookup/registry/container of services, all services must impliment 
	/// <see cref="Amplify.ComponentModel.Service"/>
	/// </summary>
	public class ServiceRegistry : IServiceProvider, IService
	{
		private Dictionary<string, object> services;

		public ServiceRegistry()
		{
			this.services = new Dictionary<string, object>();
		}


		/// <summary>
		/// Gets the service by name.
		/// </summary>
		/// <param name="name">The name of the service.</param>
		/// <returns>returns the service as an object value.</returns>
		public object this[string name]
		{
			get { return this.services[name]; }
		}

		/// <summary>
		/// Determines if the registry contains the service already.
		/// </summary>
		/// <param name="name">The name of the service.</param>
		/// <returns>returns true if the service is found, otherwise false.</returns>
		public bool Contains(string name)
		{
			return this.services.ContainsKey(name);
		}

		/// <summary>
		/// Determines if the registry contains the service already.
		/// </summary>
		/// <param name="name">The service object</param>
		/// <returns>returns true if the service is found, otherwise false.</returns>
		public bool Contains(IService service)
		{
			return this.services.ContainsKey(service.GetType().ToString());
		}

		/// <summary>
		/// Determines if the registry contains the service already.
		/// </summary>
		/// <param name="name">The type value of the service (typeof(Service)).</param>
		/// <returns>returns true if the service is found, otherwise false.</returns>
		public bool Contains(Type service)
		{
			return this.services.ContainsKey(service.ToString());
		}

		/// <summary>
		/// Adds a service to the repository by its type.
		/// </summary>
		/// <typeparam name="T">The type of the service.</typeparam>
		public void Add<T>() where T: IService 
		{
			this.services.Add(typeof(T).ToString(), Activator.CreateInstance<T>());
		}

		/// <summary>
		/// Adds a service to the repository.
		/// </summary>
		/// <param name="service">The service.</param>
		public void Add(IService service)
		{
			this.services.Add(service.GetType().ToString(), service);
		}

		/// <summary>
		/// Adds a service to the repository with a specified name.
		/// </summary>
		/// <param name="name">The name of the service.</param>
		/// <param name="service">The service.</param>
		public void Add(string name, IService service)
		{
			this.services.Add(name, service);
		}

		/// <summary>
		/// Gets the service of type T. If it does not exit, it will be created.
		/// </summary>
		/// <typeparam name="T">The type of the service that needs to be retrieved.</typeparam>
		/// <returns>returns the T service.</returns>
		public T GetService<T>()
		{
			Type get = typeof(T);
			if (!this.services.ContainsKey(get.ToString()))
				this.services.Add(get.ToString(), Activator.CreateInstance<T>());
			return (T)this.services[get.ToString()];
		}

		/// <summary>
		/// Gets the service by its type. 
		/// </summary>
		/// <param name="param">The <see cref="System.Type"/> of the service</serviceType>
		/// <returns>returns the service.</returns>
		public object GetService(Type serviceType)
		{
			if (!this.services.ContainsKey(serviceType.ToString()))
			{
				if (serviceType.GetInterface("Amplify.ComponentModel.IService", true) != null)
					this.services.Add(serviceType.ToString(), Activator.CreateInstance(serviceType));
				else
					throw new InvalidCastException(string.Format("{0} must impliment Amplify.IService", serviceType.ToString()));
			}
			return this.services[serviceType.ToString()];
		}

		/// <summary>
		/// Gets the service by a specific name.
		/// </summary>
		/// <param name="name">The name of the service.</param>
		/// <returns>returns the service.</returns>
		public object GetService(string name)
		{
			if (!this.services.ContainsKey(name))
				return null;
			return this.services[name];
		}

		/// <summary>
		/// Removes the service by a specific name.
		/// </summary>
		/// <param name="name">The name of the service.</param>
		/// <returns>returns true if the service was removed, else false.</returns>
		public bool Remove(string name)
		{
			return this.services.Remove(name);
		}

		/// <summary>
		/// Removes the service by Type
		/// </summary>
		/// <typeparam name="T">The type of the service.</typeparam>
		/// <returns>returns true if the service was removed, else false.</returns>
		public bool Remove<T>() where T: IService 
		{
			if(this.services.ContainsKey(typeof(T).ToString()))
				return this.services.Remove(typeof(T).ToString());
			return false;
		}

		/// <summary>
		/// Removes the service by instance.
		/// </summary>
		/// <param name="service">The instance of the service that is to be removed.</param>
		/// <returns>returns true if the service was removed, else false.</returns>
		public bool Remove(IService service)
		{
			Type type = service.GetType();
			if (this.services.ContainsKey(type.ToString()))
				return this.services.Remove(type.ToString());
			return false;
		}

		/// <summary>
		/// Removes the service by its type.
		/// </summary>
		/// <param name="serviceType">The <see cref="System.Type"/> of the service.</param>
		/// <returns>returns true if the service was remove, else false.</returns>
		public bool Remove(Type serviceType)
		{
			if(this.services.ContainsKey(serviceType.ToString()))
				return this.services.Remove(serviceType.ToString());
			return false;
		}
	}
}
