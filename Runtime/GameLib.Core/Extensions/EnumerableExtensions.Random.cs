using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Random = System.Random;

[SuppressMessage("ReSharper", "InconsistentNaming"), SuppressMessage("ReSharper", "CheckNamespace")]
public static partial class EnumerableExtensions
{
	public static int RandomIndex<T>(this IList<T> obj, Random rand)
	{
		if (obj.IsNullOrEmpty())
			return -1;
		var c = obj.Count;
		return c > 0 ? rand.Next(c) : -1;
	}
	
#if UNITY_2017_1_OR_NEWER
	public static int RandomIndex<T>(this IList<T> obj)
	{
		if (obj.IsNullOrEmpty())
			return -1;
		var c = obj.Count;
		return c > 0 ? UnityEngine.Random.Range(0, c) : -1;
	}
#endif

	public static T RandomItem<T>(this IList<T> obj, Random rand)
	{
		if (obj.IsNullOrEmpty())
			return default(T);
		var c = obj.Count;
		return c > 0 ? obj[rand.Next(c)] : default(T);
	}

	public static IEnumerable<T> RandomItems<T>(this IList<T> obj, int count, Random rand)
	{
		if (obj.IsNullOrEmpty())
		{
			return Enumerable.Empty<T>();
		}

		var c = obj.Count;
		if (c == 0)
		{
			return Enumerable.Empty<T>();
		}

		count = Math.Min(c, count);

		var result = new List<T>(obj);
		result.PartialShuffle(count, rand);
		return result.Take(count);
	}

#if UNITY_2017_1_OR_NEWER
	public static T RandomItem<T>(this IList<T> array)
	{
		if (array.IsNullOrEmpty())
		{
			throw new IndexOutOfRangeException("Array is empty!");
		}

		return array[UnityEngine.Random.Range(0, array.Count)];
	}

	public static IEnumerable<T> RandomItems<T>(this IList<T> obj, int count)
	{
		if (obj.IsNullOrEmpty())
		{
			return Enumerable.Empty<T>();
		}

		var c = obj.Count;
		if (c == 0)
		{
			return Enumerable.Empty<T>();
		}

		count = Math.Min(c, count);

		var result = new List<T>(obj);
		result.PartialShuffle(count);
		return result.Take(count);
	}
#endif

#if UNITY_2017_1_OR_NEWER
	public static T GetWeightedRandom<T>(this IList<T> obj, Func<T, int, float> getWeight, bool vbLog = false)
	{
		if (obj.IsNullOrEmpty())
		{
			throw new Exception($"Couldn't retrieve a weighted random value. {obj} is empty!");
		}

		var c = obj.Count;
		var sum = 0.0f;

		for (var index = 0; index < c; index++)
		{
			var value = obj[index];
			sum += getWeight(value, index);
		}

		var randomNum = UnityEngine.Random.Range(0, sum);

		if (vbLog)
		{
			UnityEngine.Debug.Log($"<color=#{UnityEngine.ColorUtility.ToHtmlStringRGB(new UnityEngine.Color(0.51f, 1f, 0.05f))}>[VB] Random weight {randomNum}</color>");
		}

		for (var index = 0; index < c; index++)
		{
			var value = obj[index];
			var weight = getWeight(value, index);
			if (randomNum < weight)
			{
				return value;
			}

			randomNum -= weight;
		}

		return obj.Last();
	}
#endif	

	/// <summary>
	/// shuffle first N items in array
	/// </summary>
	public static void PartialShuffle<T>(this IList<T> source, int count, Random random)
	{
		count = Math.Min(count, source.Count);

		for (var i = 0; i < count; i++)
		{
			var index = i + random.Next(source.Count - i);
			var tmp = source[index];
			source[index] = source[i];
			source[i] = tmp;
		}
	}

#if UNITY_2017_1_OR_NEWER
	/// <summary>
	/// shuffle first N items in array
	/// </summary>
	public static void PartialShuffle<T>(this IList<T> source, int count)
	{
		count = Math.Min(count, source.Count);

		for (var i = 0; i < count; i++)
		{
			var index = i + UnityEngine.Random.Range(0, source.Count - i);
			var tmp = source[index];
			source[index] = source[i];
			source[i] = tmp;
		}
	}
#endif

	/// <summary>
	/// shuffle all items in array
	/// </summary>
	public static void Shuffle<T>(this IList<T> source, Random random)
	{
		for (var i = 0; i < source.Count; i++)
		{
			var index = i + random.Next(source.Count - i);
			var tmp = source[index];
			source[index] = source[i];
			source[i] = tmp;
		}
	}

#if UNITY_2017_1_OR_NEWER
	/// <summary>
	/// shuffle all items in array
	/// </summary>
	public static void Shuffle<T>(this IList<T> source)
	{
		for (var i = 0; i < source.Count; i++)
		{
			var index = i + UnityEngine.Random.Range(0, source.Count - i);
			var tmp = source[index];
			source[index] = source[i];
			source[i] = tmp;
		}
	}
#endif
}