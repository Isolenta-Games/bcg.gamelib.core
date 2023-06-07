using System;
using System.Security.Cryptography;
using System.Text;

namespace GameLib.Core.Encryption
{
	public class AesEncryptor : IEncryptor, IDisposable
	{
		public static AesEncryptor Default => new("/B?E(H+MbQeThWmZ", "&E)H@McQ");

		private readonly Aes _aes;
		
		public AesEncryptor(string key, string initializationVector)
		{
			_aes = Aes.Create();
			_aes.Key = Encoding.Unicode.GetBytes(key);
			_aes.IV = Encoding.Unicode.GetBytes(initializationVector);
		}

		public string Encrypt(string plainText)
		{
			var encryptor = _aes.CreateEncryptor();

			using var memoryStream = new System.IO.MemoryStream();
			using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			using (var streamWriter = new System.IO.StreamWriter(cryptoStream))
			{
				streamWriter.Write(plainText);
			}

			var encryptedBytes = memoryStream.ToArray();
			var encryptedText = Convert.ToBase64String(encryptedBytes);
			
			return encryptedText;
		}

		public string Decrypt(string encryptedText)
		{
			var decryptor = _aes.CreateDecryptor();

			using var memoryStream = new System.IO.MemoryStream(Convert.FromBase64String(encryptedText));
			using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			using var streamReader = new System.IO.StreamReader(cryptoStream);
			
			var decryptedText = streamReader.ReadToEnd();
			return decryptedText;
		}

		public void Dispose()
		{
			_aes?.Dispose();
		}
	}
}
