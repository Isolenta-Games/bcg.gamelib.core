using System;
using System.IO.Hashing;
using System.Text;

namespace GameLib.Core.Utils
{
	public static class HashGenerator
	{
		public enum Format
		{
			CRC64,
			//SHA1 //uncomment if SHA1 will be needed
		}

		public static string GenerateHash(Format fmt, string content)
		{
			return fmt switch
			{
				Format.CRC64 => Wrap(fmt, CRC64Hash(content)),
				//Format.SHA1 => Wrap(fmt, _SHA1Hash(content)),
				_ => throw new ArgumentException($"{fmt} is not supported")
			};
		}

		private static string Wrap(Format fmt, byte[] data)
		{
			var template = $"{fmt};{Convert.ToBase64String(data)}";
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(template));
		}

		private static (Format, string) ExtractFormat(string str)
		{
			var fmt = Encoding.UTF8.GetString(Convert.FromBase64String(str)).Split(';');
			return (Enum.Parse<Format>(fmt[0]), fmt[1]);
		}

		private static byte[] CRC64Hash(string content) => Crc64.Hash(Encoding.UTF8.GetBytes(content));
		
		//private static byte[] SHA1Hash(string content) => SHA1.HashData(Encoding.UTF8.GetBytes(content));
	}
}
