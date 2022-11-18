using System;
using System.Numerics;

namespace GameLib.Core.Extensions
{
	public static class BigIntegerExtensions
	{
		public static double Divide(this BigInteger source, BigInteger target)
		{
			return Math.Exp(BigInteger.Log(source) - BigInteger.Log(target));
		}

		public static double ToDouble(this BigInteger source)
		{
			return (double)source;
		}
	}
}