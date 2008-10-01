//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DefyTheGrind.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Web.UI.WebControls;

	using Amplify.ActiveRecord;
	using Amplify.ObjectModel;
	using Amplify.Data.Validation;

	[Serializable]
	public class Event : Amplify.ActiveRecord.Base<Event>
	{

		public Event() : base() { 
		
			Mode = "LocalSqlServer";
			this["Id"] = Guid.NewGuid();
		}

		[Column(IsPrimaryKey = true, Default = typeof(GuidFactory))]
		public Guid Id
		{
			get { return (Guid)this.Get("Id"); }
		}

		[Column, ValidatePresence, ValidateFormat(With = @"([\d\s\w])*")]
		public string Name
		{
			get { return (string)this.GetString("Name"); }
			set { this.Set("Name", value); }
		}

		[Column(Default = 0.00), ValidatePresence]
		public Decimal Price
		{
			get { return (decimal)this.Get("Price"); }
			set { this.Set("Price", value); }
		}

		[Column(Default = typeof(DateTimeNowFactory)), ValidatePresence]
		public DateTime TicketSalesStartAt
		{
			get { return (DateTime)this.Get("TicketSalesStartAt"); }
			set { this.Set("TicketSalesStartAt", value); }
		}

		[Column(Default = typeof(DateTimeNowFactory)), 
			ValidateComparison(
				PropertyToCompare = "TicketSalesStartAt", 
				Operator = ValidationCompareOperator.GreaterThan), ValidatePresence ]
		public DateTime StartsAt
		{
			get { return (DateTime)this.Get("StartsAt"); }
			set { this.Set("StartsAt", value); }
		}

		[Column(Default = typeof(EmptyGuidFactory))] 
		public Guid? VenueId
		{
			get { return (Guid?)this.Get("VenueId"); }
			set { this.Set("VenueId", value); }
		}

		[Column(Default = typeof(EmptyGuidFactory))] 
		public Guid? OrganizationId
		{
			get { return (Guid?)this.Get("OrganizationId"); }
			set { this.Set("OrganizationId", value); }
		}
	}
}
