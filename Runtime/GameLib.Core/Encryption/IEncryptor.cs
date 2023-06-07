namespace GameLib.Core.Encryption
{
	public interface IEncryptor
	{
		string Encrypt(string plainText);
		string Decrypt(string encryptedText);
	}
}
