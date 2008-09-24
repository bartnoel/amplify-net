using System;
using System.Collections.Generic;
using System.Text;

namespace Amplify
{
	/// <summary>
	/// Executes a command with a variable number of arguements. 
	/// </summary>
	/// <remarks>
	///		<para>
	///			The <strong>ICommand</strong> interface serves to help 
	///			impliment the Command Pattern.
	///		</para>
	/// </remarks>
	public interface ICommand
	{
		/// <summary>
		/// Executes the specified arguments.
		/// </summary>
		/// <param name="arguments">The arguments.</param>
		/// <returns></returns>
		object Execute(params object[] arguments);
	}
}
