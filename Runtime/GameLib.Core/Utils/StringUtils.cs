using System;
using System.Text;

namespace GameLib.Core.Utils
{
	public class StringUtils
	{
		public static string GetRandomString(int length)
		{
			var random = new Random();
			
			var sb = new StringBuilder();
			char[] chars = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

			for (var i = 0; i < length; i++)
			{
				var index = random.Next(chars.Length);
				sb.Append(chars[index]);
			}

			return sb.ToString();
		}
	}
}
