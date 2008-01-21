

namespace Amplify.Office
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Amplify.Office.Word;

	public class Officev2003
	{
		public static IWordApplication StartWordInstance()
		{
			return new v2003.Word.Application();
		}

		public static IWordApplication StartWordInstance(bool automation)
		{
			return new v2003.Word.Application(automation);
		}

		public static IWordApplication StartWordInstance(bool automation, bool trackProcess)
		{
			return new v2003.Word.Application(automation, trackProcess);
		}
	}
}
