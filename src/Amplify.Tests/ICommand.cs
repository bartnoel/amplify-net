//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) CompanyName.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using MbUnit.Framework;
	using Gallio.Framework;

	[
		Describe(typeof(ICommand)),
		InContext(" a basic example of how ICommand is used. "),
		Tag(Tags.Instructional),
		By("Michael Herndon", "mherndon@opensourceconnections.com", "www.amptools.net")
	]
	public class ICommandSpecification : Spec
	{

		public class Command : ICommand
		{
			public bool IsExecuted { get; set; }

			public void Execute()
			{
				this.IsExecuted = true;
			}
		}

		[It, Should(" has an execute method. ")]
		public void InvokeExecute()
		{
			Command command = new Command();
			command.IsExecuted.ShouldBeFalse();
			command.Execute();
			command.IsExecuted.ShouldBeTrue();
		}
	}
}
