//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Text;

	[SuppressMessageAttribute("Microsoft.Naming", "CA1709", Justification = "Db should be Db, not DB.")]
	public enum DbType
	{
		/// <summary>
		/// Not a database type
		/// </summary>
		None,
		/// <summary>
		/// a primary key, defaults to int with increment seed.
		/// </summary>
		PrimaryKey,
		/// <summary>
		/// a integer primary key that auto increments
		/// </summary>
		PrimaryKeyInt,
		/// <summary>
		/// a guid primary key 
		/// </summary>
		PrimaryKeyGuid,
		/// <summary>
		/// varchar
		/// </summary>
		AnsiString,
		/// <summary>
		/// text field
		/// </summary>
		AnsiText,
		/// <summary>
		/// binary field
		/// </summary>
		Binary,
		/// <summary>
		/// largy binary field
		/// </summary>
		Blob,
		/// <summary>
		/// bit field
		/// </summary>
		Boolean,
		/// <summary>
		/// bit field
		/// </summary>
		Byte,
		/// <summary>
		/// currency field
		/// </summary>
		Currency,
		/// <summary>
		/// date field
		/// </summary>
		Date, 
		/// <summary>
		/// date time field
		/// </summary>
		DateTime,
		/// <summary>
		/// date time 2 field
		/// </summary>
		DateTime2,
		/// <summary>
		/// date time offset
		/// </summary>
		DateTimeOffset,
		/// <summary>
		/// decimal field
		/// </summary>
		Decimal,
		/// <summary>
		/// dobule field
		/// </summary>
		Double,
		/// <summary>
		/// float field
		/// </summary>
		Float,
		/// <summary>
		/// geography field
		/// </summary>
		Geography,
		/// <summary>
		/// geometry field
		/// </summary>
		Geometry,
		/// <summary>
		/// guid field
		/// </summary>
		Guid,
		/// <summary>
		/// integer field (32 bit)
		/// </summary>
		Integer,
		/// <summary>
		/// integer field (16 bit)
		/// </summary>
		Int16,
		/// <summary>
		/// integer field (32 bit)
		/// </summary>
		Int32,
		/// <summary>
		/// integer field (64 bit)
		/// </summary>
		Int64,
		/// <summary>
		/// real field
		/// </summary>
		Real,
		/// <summary>
		/// row field (mssql timestamp/row identifier)
		/// </summary>
		RowVersion,
		/// <summary>
		/// single field
		/// </summary>
		Single,
		/// <summary>
		/// small datetime 
		/// </summary>
		SmallDateTime,
		/// <summary>
		/// nvarchar field
		/// </summary>
		String,
		/// <summary>
		/// ntext field
		/// </summary>
		Text,
		/// <summary>
		/// time field
		/// </summary>
		Time,
		/// <summary>
		/// timestamp (alias for datetime)
		/// </summary>
		[SuppressMessageAttribute("Microsoft.Naming", "CA1702", Justification = "FxCop can't spell")]
		TimeStamp,
		/// <summary>
		/// unsigned integer field (16 bit)
		/// </summary>
		UInt16,
		/// <summary>
		/// unsigned integer field (32 bit)
		/// </summary>
		UInt32,
		/// <summary>
		/// unsigned integer field (64 bit)
		/// </summary>
		UInt64,
		/// <summary>
		/// user object
		/// </summary>
		User,
		/// <summary>
		/// var numeric field
		/// </summary>
		VarNumeric,
		/// <summary>
		/// xml field
		/// </summary>
		Xml
		
	}
}
