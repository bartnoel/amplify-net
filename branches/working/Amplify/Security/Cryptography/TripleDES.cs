//-----------------------------------------------------------------------
// <copyright file="_Documentation/Copyrights.cs" company="Entry7Media">
//     Copyright (c) Entry7Media.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Amplify.Security.Cryptography 
{
	/// <summary>
	/// 
	/// </summary>
	// TODO: Fix TripleDes Encrption. 
	public class TripleDES: IEncryptable 
	{
		private Byte[] key;
		private Byte[] iv;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="iv"></param>
		public TripleDES(Byte[] key, Byte[] iv)
		{
			this.key = key;
			this.iv = iv;
		}

		public TripleDES(string key, Byte[] iv)
		{
			PasswordDeriveBytes db = new PasswordDeriveBytes(key, new byte[0]);
			this.key = db.GetBytes(16);
			this.iv = iv;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public string Encrypt(string text)
		{
			if (string.IsNullOrEmpty(text) || text.Trim() == "") 
				return "";

			Byte[] inputBytes = new UnicodeEncoding().GetBytes(text);
			
			using (MemoryStream encryptedStream = new MemoryStream()) 
			{
				using (CryptoStream cryptoStream = 
					new CryptoStream(
						encryptedStream,
						GetCryptoTransformFromEncryptor(),
						CryptoStreamMode.Write)
					) 
				{
					cryptoStream.Write(inputBytes, 0, inputBytes.Length);
					cryptoStream.Close();
					encryptedStream.Close();
				}
				return (new UTF32Encoding().GetString(encryptedStream.ToArray()));
			}
		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public string Decrypt(string text) 
		{
			if (string.IsNullOrEmpty(text) || text.Trim() == "") 
				return "";

			Byte[] inputBytes = new UnicodeEncoding().GetBytes(text);

			using (MemoryStream decryptedStream = new MemoryStream()) 
			{
				using (CryptoStream cryptoStream =
					new CryptoStream(
							decryptedStream,
							GetCryptoTransformFromDecryptor(),
							CryptoStreamMode.Write)
					) 
				{
					cryptoStream.Write(inputBytes, 0, inputBytes.Length);
					cryptoStream.Close();
					decryptedStream.Close();
				}
				return (new UTF8Encoding().GetString(decryptedStream.ToArray()));
			}
			
		}

		private ICryptoTransform GetCryptoTransformFromEncryptor() 
		{
			return (new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv));
		}

		private ICryptoTransform GetCryptoTransformFromDecryptor() 
		{
			return (new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv));
		}

		private Byte[] GetInputBytes(string text) 
		{
			return (new UTF8Encoding().GetBytes(text));
		}


		#region IDisposable Members

		public void Dispose() 
		{
			this.iv = null;
			this.key = null;
		}

		#endregion
	}
}
