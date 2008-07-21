//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Security.Cryptography 
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	/// <summary>
	/// Interface/Contract for ecryption objects. 
	/// </summary>
	public interface IEncryptable: IDisposable 
	{
		/// <summary>
		/// Encrypts the specified text.
		/// </summary>
		/// <param name="text">The text that is to be encrypted.</param>
		/// <returns>The encrypted value of the string.</returns>
		string Encrypt(string text);
		/// <summary>
		/// Decrypts the specified text.
		/// </summary>
		/// <param name="text">The text value that is to be decrypted.</param>
		/// <returns>The decrypted value of the string.</returns>
		string Decrypt(string text);
	}
}
