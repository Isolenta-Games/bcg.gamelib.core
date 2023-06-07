namespace GameLib.Core.Encryption
{
	public class DummyEncryptor : IEncryptor
	{
		public string Encrypt(string plainText)
		{
			return plainText;
		}

		public string Decrypt(string encryptedText)
		{
			return encryptedText;
		}
	}
}