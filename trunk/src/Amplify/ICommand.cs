using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
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
