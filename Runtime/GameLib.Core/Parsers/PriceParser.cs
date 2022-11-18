using System;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameLib.Core.Parsers
{
	public static class PriceParser
	{
		private static readonly char[] StartNum = "01234567890".ToCharArray();
		private static readonly char[] BeforePoint = "01234567890,. \u00A0".ToCharArray();
		private static readonly char[] AfterPoint = "01234567890".ToCharArray();
		private static readonly char[] WhiteSpaces = " \u00A0".ToCharArray();

		private enum ParsingState
		{
			SearchStartNum,
			SearchPoint,
			SearchEndNumber,
		}
		
		public static string MakeFakePrice(string originalPrice, float mult, bool roundToInt = false)
		{
			// matches:
			// "0.33" 
			// "1 000,30"
			// "100500"
			// "1,000.30"

			var sb = new StringBuilder(32);

			var s = ParsingState.SearchStartNum;
			
			foreach (var ch in originalPrice)
			{
				var needStop = false;
				switch (s)
				{
					case ParsingState.SearchStartNum:
						if (StartNum.Contains(ch))
						{
							sb.Append(ch);
							s = ParsingState.SearchPoint;
						}
						break;

					case ParsingState.SearchPoint:
						if (BeforePoint.Contains(ch))
						{
							sb.Append(ch);
							if (ch == '.')
							{
								s = ParsingState.SearchEndNumber;
							}
						}
						else
						{
							needStop = true;
						}
						break;

					case ParsingState.SearchEndNumber:
						if (AfterPoint.Contains(ch))
						{
							sb.Append(ch);
						}
						else
						{
							needStop = true;
						}
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
				
				if (needStop)
				{
					break;
				}
			}
			
			var resultString = sb.ToString().Trim();
			var val = resultString.Replace(" ", string.Empty);

			foreach (var v in resultString)
			{
				if (WhiteSpaces.Contains(v))
				{
					val = val.Replace(v.ToString(), string.Empty);
				}
			}
			
			if (val.Contains(',') && val.Contains('.'))
			{
				val = val.Replace(",", string.Empty);
			}
			else
			{
				val = val.Replace(',', '.');
			}

			if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out var resValue))
			{
				resValue *= mult;
				if (roundToInt)
				{
					resValue = (int)resValue;
				}
				var formatInfo = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
				formatInfo.CurrencySymbol = string.Empty;

				return originalPrice.Replace(resultString, resValue.ToString("C", formatInfo).Trim());
			}

			return string.Empty;
		}
		
	}
}