using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using GameLib.Core.Utils;

[SuppressMessage("ReSharper", "CheckNamespace")]
public static class RandomExtensions
{
	/// <summary>
	/// return [0..1] - inclusive 
	/// </summary>
	public static float RandomK(this Random rand)
	{
		return (float)Math.Round(rand.NextDouble(), 8);
	}

	/// <summary>
	/// return [0..100] - inclusive 
	/// </summary>
	public static float RandomPercent(this Random rand)
	{
		return rand.RandomK() * 100.0f;
	}

	/// <summary>
	/// return [min..max] - inclusive 
	/// </summary>
	public static int RandomRange(this Random rand, int min, int max)
	{
		if (min == max) return min;
		if (min > max) MathEx.Swap(ref min, ref max);

		return MathEx.RoundToInt(MathEx.Lerp(min, max, rand.RandomK()));
	}

	/// <summary>
	/// return [min..max] - inclusive 
	/// </summary>
	public static long RandomRange(this Random rand, long min, long max)
	{
		if (min == max) return min;
		if (min > max) MathEx.Swap(ref min, ref max);

		return MathEx.RoundToLong(MathEx.Lerp(min, max, rand.RandomK()));
	}

	/// <summary>
	/// return random number [min, max] with minimal step between values. for ex. [10, 100] step=10 returns 10, 20, 50, ...
	/// </summary>
	public static int RandomRange(this Random rand, int min, int max, int step)
	{
		if (min == max) return min;
		if (step <= 1) return RandomRange(rand, min, max);

		if (min > max) MathEx.Swap(ref min, ref max);

		min /= step;
		max /= step;

		return MathEx.RoundToInt(MathEx.Lerp(min, max, rand.RandomK()) * step);
	}

	/// <summary>
	/// return [min..max] - inclusive 
	/// </summary>
	public static float RandomRange(this Random rand, float min, float max)
	{
		if (min > max) MathEx.Swap(ref min, ref max);
		return rand.RandomK() * (max - min) + min;
	}

	/// <summary>
	/// return [min..max] - inclusive 
	/// </summary>
	public static double RandomRange(this Random rand, double min, double max)
	{
		if (min > max) MathEx.Swap(ref min, ref max);
		return rand.RandomK() * (max - min) + min;
	}
}
