namespace Amplify.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class PluginsCollection : System.Configuration.ConfigurationElementCollection  
	{

		protected override object GetElementKey(System.Configuration.ConfigurationElement element)
		{
			return ((PluginElement)element).Type;
		}

		protected override System.Configuration.ConfigurationElement CreateNewElement()
		{
			return new PluginElement();
		}

		protected override System.Configuration.ConfigurationElement CreateNewElement(string elementName)
		{
			Type type = Type.GetType(elementName);
			return (System.Configuration.ConfigurationElement)Activator.CreateInstance(type);
		}

		public void Add(PluginElement element)
		{
			this.BaseAdd(element);
		}

		public PluginElement this[string name]
		{
			get
			{
				foreach (PluginElement item in this)
					if (item.Name.ToLower() == name.ToLower())
						return item;
				return null;
			}
		}

		public PluginElement this[Type type]
		{
			get
			{
				foreach (PluginElement item in this)
					if (item.GetType() == type)
						return item;

				return null;
			}
		}

		public int Count
		{
			get { return base.Count; }
		}

		public int IndexOf(PluginElement element)
		{
			return BaseIndexOf(element);
		}

		public void Remove(PluginElement element)
		{
			this.BaseRemove(element);
		}

		public void RemoveAt(int index)
		{
			this.BaseRemoveAt(index);
		}
		public void Clear()
		{
			this.BaseClear();
		}
	}
}
