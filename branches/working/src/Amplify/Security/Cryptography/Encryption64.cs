//-----------------------------------------------------------------------
// <copyright file="Copyright.cs" author="Michael Herndon">
//     Copyright (c) Michael Herndon.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Amplify.Security.Cryptography
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Text;
	using System.Security.Cryptography;

	public class Encryption64 : IEncryptable 
	{
		private Byte[] key;
		private Byte[] iv;

		public Encryption64(string key, Byte[] iv)
		{
			this.key = Encoding.UTF8.GetBytes(key.Substring(0, 8));
			this.iv = iv;
		}

		public Encryption64(Byte[] key, Byte[] iv) 
		{
			this.key = key;
			this.iv = iv;
		}

		public string Encrypt(string text) 
		{
			if (string.IsNullOrEmpty(text) || text.Trim() == "") return "";
			
			Byte[] inputBytes = GetBytes(text);
			
			using (MemoryStream mstream1 = new MemoryStream()) 
			{
				using (CryptoStream cstream1 = 
					new CryptoStream(
						mstream1,
						GetCryptoTransformFromEncryptor(),
						CryptoStreamMode.Write)
					) 
				{
					cstream1.Write(inputBytes, 0, inputBytes.Length);
					cstream1.FlushFinalBlock();
				}

				return Convert.ToBase64String(mstream1.ToArray());
			}
		}

		public string Decrypt(string text) 
		{
			if (string.IsNullOrEmpty(text) || text.Trim() == "") 
				return "";
		
			Byte[] inputBytes = Convert.FromBase64String(text);
			
			using( MemoryStream mstream1 = new MemoryStream()){
				using (CryptoStream cstream1 =
					new CryptoStream(
						mstream1,
						GetCtyptoTransformFromDecryptor(),
						CryptoStreamMode.Write)
					) {
					cstream1.Write(inputBytes, 0, inputBytes.Length);
					cstream1.FlushFinalBlock();
				}
				return Encoding.UTF8.GetString(mstream1.ToArray());
			}
		}

		private ICryptoTransform GetCryptoTransformFromEncryptor() 
		{
			return (new DESCryptoServiceProvider().CreateEncryptor(key, iv));
		}

		private ICryptoTransform GetCtyptoTransformFromDecryptor() 
		{
			return (new DESCryptoServiceProvider().CreateDecryptor(key, iv));
		}

		private Byte[] GetBytes(string text) 
		{
			return Encoding.UTF8.GetBytes(text);
		}

		#region IDisposable Members

		public void Dispose() {
			this.iv = null;
			this.key = null;
		}

		#endregion
	}
}
