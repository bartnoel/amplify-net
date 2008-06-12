using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amplify.Data.Entity
{
	public class EntityAttribute : ITrackChanges 
	{
		private object value = DecoratedProperty.UnsetValue;
		private object initialValue = null;
		private IPropertyInfo property;


		public EntityAttribute(IPropertyInfo property) :base(
			property, property.DefaultValue)
		{

		}

		public EntityAttribute(IPropertyInfo property, object defaultValue)
		{
			this.property = property;
			this.initialValue = defaultValue;
		}

		public string Name
		{
			get { return this.property.Name; }
		}

		public object Value
		{
			get {
				if (value == DecoratedProperty.UnsetValue)
					return this.initialValue;
				return value;
			}
			set {
				this.value = value;
			}
		}

		public bool IsUnset
		{
			get { return value == DecoratedProperty.UnsetValue; }
		}

		#region ITrackChanges Members

		public bool IsModified
		{
			get {
				if (this.IsUnset)
					return false;
				ITrackChanges dependent = value as ITrackChanges;
				if (dependent != null)
					return dependent.IsModified;

				return (initialValue != value);
			}
		}

		public void Save()
		{
			initialValue = value;
		}

		public bool IsMarkedForDeletion
		{
			get {
				ITrackChanges dependent = value as ITrackChanges;
				if (dependent != null)
					return dependent.IsMarkedForDeletion;
				return false;
			}
		}

		public bool IsNew
		{
			get {
				ITrackChanges dependent = value as ITrackChanges;
				if (dependent != null)
					return dependent.IsNew;
				return false;
			}
		}

		public bool IsValid
		{
			get {
				ITrackChanges dependent = value as ITrackChanges;
				if (dependent != null)
					return dependent.IsValid;
				return true;
			}
		}

		public bool IsEntityValid
		{
			get {
				return this.IsValid;
			}
		}

		public bool IsEntityModified
		{
			get {
				return this.IsModified;
			}
		}

		#endregion
	}
}
