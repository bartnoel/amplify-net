//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Security 
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class Password 
	{
		/// <summary>
		/// Generates a password of the specified length.
		/// </summary>
		/// <param name="passwordLength"> The length of the password to generate. </param>
		/// <returns> The generated password string. </returns>
		public static string Generate(int passwordLength)
		{
			StringBuilder sb = new StringBuilder("ABCDEFGHJKMNPQRSTUVWXYZ1234567890abcdefghjkmnpqrstuvwxyz#@%$+");
			int range = sb.Length;
			Random random = new Random();
			StringBuilder password = new StringBuilder(passwordLength);

			for (int i = 0; i < passwordLength; i++) 
			{
				Char randomChar = sb[random.Next(1, sb.Length)];
				password.Append(randomChar);
			}

			return password.ToString();
		}

	}
}
