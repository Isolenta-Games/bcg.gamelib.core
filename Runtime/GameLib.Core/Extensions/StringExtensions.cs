using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

[SuppressMessage("ReSharper", "CheckNamespace")]
public static class StringExtensions
{
	/// <summary>
	/// return true, if string is null or empty
	/// </summary>
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	/// <summary>
	/// clamp string value to maximum length
	/// </summary>
	public static string Take(this string s, int maxLength)
	{
		if (s.IsNullOrEmpty())
		{
			return string.Empty;
		}

		return s.Length > maxLength ? s.Substring(0, maxLength) : s;
	}

	/// <summary>
	/// clamp string value to maximum length with '...'
	/// </summary>
	public static string ClampWithEllipsis(this string s, int maxLength)
	{
		if (s.IsNullOrEmpty() || maxLength <= 0)
		{
			return string.Empty;
		}

		maxLength = Math.Min(s.Length, maxLength);

		if (maxLength <= 3)
		{
			return s.Substring(0, maxLength);
		}

		return s.Length > maxLength ? s.Substring(0, maxLength - 3) + "..." : s;
	}

	/// <summary>
	/// case insensitive replace
	/// </summary>
	public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
	{
		if (str.IsNullOrEmpty())
		{
			return string.Empty;
		}

		if (oldValue.IsNullOrEmpty())
		{
			return str;
		}

		newValue ??= string.Empty;
		
		if (comparison == StringComparison.CurrentCulture)
		{
			return str.Replace(oldValue, newValue);
		}

		var sb = new StringBuilder(str.Length);

		var previousIndex = 0;
		var index = str.IndexOf(oldValue, comparison);

		while (index != -1)
		{
			sb.Append(str.Substring(previousIndex, index - previousIndex));
			sb.Append(newValue);
			index += oldValue.Length;

			previousIndex = index;
			index = str.IndexOf(oldValue, index, comparison);
		}

		sb.Append(str.Substring(previousIndex));

		return sb.ToString();
	}
	
	/// <summary>
	/// replace character in a string (slow) 
	/// </summary>
	public static string ReplaceAt(this string input, int index, char newChar)
	{
		if (input == null)
		{
			throw new ArgumentNullException(nameof(input));
		}
		
		var chars = input.ToCharArray();
		chars[index] = newChar;
		return new string(chars);
	}	
}
