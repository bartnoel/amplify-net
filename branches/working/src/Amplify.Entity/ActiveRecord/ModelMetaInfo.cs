//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.ActiveRecord
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Reflection;

	using Amplify.Linq;
	using Amplify.ComponentModel;

	public class ModelMetaInfo
	{
		private List<TableAttribute> tables;
		private List<ColumnAttribute> columns;
		private List<AssocationAttribute> associations;
		private List<IValidationRule> rules;


		public List<IValidationRule> Rules
		{
			get {
				if (this.rules == null)
					this.rules = new List<IValidationRule>();
				return this.rules;
			}
		}

		public List<TableAttribute> Tables
		{
			get {
				if (this.tables == null)
					this.tables = new List<TableAttribute>();
				return this.tables;
			}
		}

		public List<ColumnAttribute> Columns
		{
			get {
				if (this.columns == null)
					this.columns = new List<ColumnAttribute>();
				return this.columns;
			}
		}

		public List<AssocationAttribute> Associations
		{
			get
			{
				if (this.associations == null)
					this.associations = new List<AssocationAttribute>();
				return this.associations;
			}
		}

		private static Dictionary<Type, ModelMetaInfo> repository;


		internal static Dictionary<Type, ModelMetaInfo> Repository
		{
			get {
				if (repository == null)
					repository = new Dictionary<Type, ModelMetaInfo>();
				return repository;
			}
		}

		public PrimaryTableAttribute PrimaryTable
		{
			get;
			set;
		}

		public string[] PrimaryKeys { get; set; }

		public static bool Contains(Type type)
		{
			return Repository.ContainsKey(type);
		}

		public static void Add(Type type)
		{
			ModelMetaInfo info = new ModelMetaInfo();
			Repository.Add(type, info);
		}

		

		public static ModelMetaInfo Get(Type type)
		{
			ModelMetaInfo info = null;

			if (!Repository.ContainsKey(type))
			{
				info = new ModelMetaInfo();
				Repository.Add(type, info);
			}
			else 
				info = Repository[type];
			
			return info;
		}
	}
}
