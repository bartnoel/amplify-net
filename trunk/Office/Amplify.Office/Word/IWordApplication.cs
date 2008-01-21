//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) MichaelHerndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Office.Word
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public interface IWordApplication : IDisposable 
	{
		event WindowActivateEventHandler WindowActivate;
		IDocumentsList Documents { get; }
		IWindowList Windows { get; }
		void Quit();

	}
}
