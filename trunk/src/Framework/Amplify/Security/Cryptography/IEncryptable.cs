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


	public interface IEncryptable: IDisposable 
	{
		string Encrypt(string text);
		string Decrypt(string text);
	}
}
