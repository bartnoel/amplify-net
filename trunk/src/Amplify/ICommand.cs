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

	/// <summary>
	/// A basic contract to help with creating the command pattern
	/// within an application.  
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Executes a command.
		/// </summary>
		void Execute();
	}
}
