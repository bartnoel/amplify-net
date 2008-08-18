using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.Mapping;
using System.Data.Metadata.Edm;

namespace Amplify.Models.Objects
{
	public static class Mixins {

		private static readonly List<EntityMetaData> metadata = new Dictionary<Type, EntityMetaData>();


		internal static string GetEntitySetName<T>(this ObjectContext context) where T : EntityObject
		{
			Type type = typeof(T);
			if (!metadata.Contains(type))
			{
				EntityMetaData data = new EntityMetaData();
				EntityContainer container = context.MetadataWorkspace.GetEntityContainer(context.GetType().Name, DataSpace.CSpace);
				   container.BaseEntitySets.FirstOrDefault(es => es.ElementType.ToString() == type.FullName).Name;
			}
			return metadata[type].EntitySetName;
		}

	}
}
