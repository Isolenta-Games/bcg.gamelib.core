using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

[SuppressMessage("ReSharper", "InconsistentNaming"), SuppressMessage("ReSharper", "CheckNamespace")]
public static partial class EnumerableExtensions
{
	/// <summary>
	/// check if index is valid
	/// </summary>
	public static bool IsValidIndex<SourceT>(this IList<SourceT> source, int index)
	{
		return source != null && index >= 0 && index < source.Count;
	}

	/// <summary>
	/// check if index is valid
	/// </summary>
	public static bool IsValidIndex<SourceT>(this SourceT[] source, int index)
	{
		return source != null && index >= 0 && index < source.Length;
	}

	/// <summary>
	/// return index in range [0..source.Length - 1]
	/// </summary>
	public static int ClampIndex<SourceT>(this IList<SourceT> source, int index)
	{
		if (source == null)
			return index;
		if (index < 0)
			return 0;
		if (index >= source.Count)
			return source.Count - 1;
		return index;
	}

	/// <summary>
	/// return index in range [0..source.Length - 1]
	/// </summary>
	public static int ClampIndex<SourceT>(this SourceT[] source, int index)
	{
		if (source == null)
			return index;
		if (index < 0)
			return 0;
		if (index >= source.Length)
			return source.Length - 1;
		return index;
	}
	
	/// <summary>
	/// return value, contained in dictionary, or create, or add new value
	/// </summary>
	public static ValueT GetOrAddValue<KeyT, ValueT>(this Dictionary<KeyT, ValueT> obj, KeyT key) where ValueT : class, new()
	{
		if (obj.TryGetValue(key, out var result))
			return result;

		result = new ValueT();

		obj.Add(key, result);

		return result;
	}
	
	/// <summary>
	/// return value, contained in dictionary, or create, or add new value
	/// </summary>
	public static ValueT GetOrAddValue<KeyT, ValueT>(this Dictionary<KeyT, ValueT> obj, KeyT key, Func<ValueT> creator) where ValueT : class
	{
		if (obj.TryGetValue(key, out var result))
			return result;

		result = creator();

		obj.Add(key, result);

		return result;
	}
	
	/// <summary>
	/// return value, contained in dictionary, or create, or add new value
	/// </summary>
	public static ValueT GetOrAddValue<ValueT>(this ICollection<ValueT> obj, Func<ValueT, bool> selector) where ValueT : class, new()
	{
		var result = obj.FirstOrDefault(selector);
		if (result == null)
		{
			result = new ValueT();
			obj.Add(result);
		}

		return result;
	}

	public static ValueT GetOrAddValue<ValueT>(this ICollection<ValueT> obj, Func<ValueT, bool> selector, Func<ValueT> creator) where ValueT : class
	{
		var result = obj.FirstOrDefault(selector);
		if (result == null)
		{
			result = creator();
			obj.Add(result);
		}

		return result;
	}
	
	/// <summary>
	/// add to list only unique items
	/// </summary>
	public static ISet<T> AddOnce<T>(this ISet<T> obj, T item)
	{
		if (!obj.Contains(item))
		{
			obj.Add(item);
		}

		return obj;
	}
	
	/// <summary>
	/// add to list only unique items
	/// </summary>
	public static IList<T> AddOnce<T>(this IList<T> obj, T item)
	{
		if (obj.IndexOf(item) < 0)
		{
			obj.Add(item);
		}

		return obj;
	}
	
	/// <summary>
	/// add to list only unique items from other List
	/// </summary>
	public static IList<T> AddOnce<T>(this IList<T> obj, IEnumerable<T> other)
	{
		if (other == null)
			return obj;

		foreach (var t in other)
		{
			obj.AddOnce(t);
		}

		return obj;
	}

	public static T RemoveFirst<T>(this IList<T> obj)
	{
		if (obj.Count == 0)
			throw new IndexOutOfRangeException("List is empty!");

		var result = obj[0];
		obj.RemoveAt(0);

		return result;
	}

	public static T RemoveFirst<T>(this IList<T> obj, Func<T, bool> predicate)
	{
		if (obj.Count == 0)
			throw new IndexOutOfRangeException("List is empty!");

		for (int i = 0; i < obj.Count; i++)
		{
			var result = obj[i];
			if (predicate(result))
			{
				obj.RemoveAt(i);
				return result;
			}
		}

		return default(T);
	}

	public static T RemoveLast<T>(this IList<T> obj)
	{
		var c = obj.Count;
		if (c == 0)
			throw new IndexOutOfRangeException("List is empty!");

		var result = obj[c - 1];
		obj.RemoveAt(c - 1);

		return result;
	}

	public static T RemoveLast<T>(this IList<T> obj, Func<T, bool> predicate)
	{
		if (obj.Count == 0)
			throw new IndexOutOfRangeException("List is empty!");

		for (int i = obj.Count - 1; i >= 0; i--)
		{
			var result = obj[i];
			if (predicate(result))
			{
				obj.RemoveAt(i);
				return result;
			}
		}

		return default(T);
	}

	public static void Swap<T>(this IList<T> obj, int i1, int i2)
	{
		(obj[i1], obj[i2]) = (obj[i2], obj[i1]);
	}

	[ContractAnnotation("obj:null => true; obj:notnull=>false")]
	public static bool IsNullOrEmpty<K, T>(this Dictionary<K, T> obj)
	{
		return obj == null || obj.Count == 0;
	}

	[ContractAnnotation("obj:null => true; obj:notnull=>false")]
	public static bool IsNullOrEmpty<T>(this ICollection<T> obj)
	{
		return obj == null || obj.Count == 0;
	}

	[ContractAnnotation("obj:null => true; obj:notnull=>false")]
	public static bool IsNullOrEmpty<T>(this T[] obj)
	{
		return obj == null || obj.Length == 0;
	}

	[ContractAnnotation("obj:null => true; obj:notnull=>false")]
	public static bool IsNullOrEmpty<T>(this IEnumerable<T> obj)
	{
		return obj == null || !obj.Any();
	}

	[ContractAnnotation("obj:null => false; obj:notnull=>true")]
	public static bool NotEmpty<K, T>(this Dictionary<K, T> obj)
	{
		return obj != null && obj.Count > 0;
	}

	[ContractAnnotation("obj:null => false; obj:notnull=>true")]
	public static bool NotEmpty<T>(this ICollection<T> obj)
	{
		return obj != null && obj.Count > 0;
	}

	[ContractAnnotation("obj:null => false; obj:notnull=>true")]
	public static bool NotEmpty<T>(this T[] obj)
	{
		return obj != null && obj.Length > 0;
	}

	/// <summary>
	/// return index of specific item or -1, if not found
	/// </summary>
	public static int IndexOf<T>(this IList<T> obj, Func<T, bool> predicate)
	{
		if (obj.IsNullOrEmpty())
			return -1;

		var c = obj.Count;
		for (var i = 0; i < c; i++)
		{
			if (predicate(obj[i]))
				return i;
		}

		return -1;
	}

	/// <summary>
	/// return index of specific item or -1, if not found
	/// </summary>
	public static int LastIndexOf<T>(this IList<T> obj, Func<T, bool> predicate)
	{
		if (obj.IsNullOrEmpty())
			return -1;

		var c = obj.Count;
		for (var i = c - 1; i >= 0; i--)
		{
			if (predicate(obj[i]))
				return i;
		}

		return -1;
	}

	/// <summary>
	/// return index of specific item or -1, if not found
	/// </summary>
	public static int IndexOf<T>(this IEnumerable<T> obj, Func<T, bool> predicate)
	{
		if (obj.IsNullOrEmpty())
		{
			return -1;
		}

		var index = 0;
		foreach (var item in obj)
		{
			if (predicate(item))
			{
				return index;
			}
			++index;
		}

		return -1;
	}

	public static string JoinToString<T>(this T[] array, string separator = ",")
	{
		return array != null && array.Length > 0 ? string.Join(separator, array.Select(x => x?.ToString())) : string.Empty;
	}

	public static string JoinToString<T>(this IEnumerable<T> array, string separator = ",")
	{
		return array != null ? string.Join(separator, array.Select(x => x?.ToString())) : string.Empty;
	}

	/// <summary>
	/// dump types of instances (for debugging)
	/// </summary>
	public static string DumpTypes<T>(this IEnumerable<T> array)
	{
		return "[" + (array != null ? string.Join(",", array.SelectToArray(x => x.GetType().Name)) : string.Empty) + "]";
	}

	public static bool TryDequeue<T>(this Queue<T> queue, out T result)
	{
		if (queue.Count == 0)
		{
			result = default;
			return false;
		}

		result = queue.Dequeue();
		return true;
	}
	
	/// <summary>Convert a colletion to a HashSet.</summary>
	public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
	{
		return source != null ? new HashSet<T>(source) : new HashSet<T>();
	}

	/// <summary>Convert a colletion to a HashSet.</summary>
	public static HashSet<T> ToHashSet<T>(
		this IEnumerable<T> source,
		IEqualityComparer<T> comparer)
	{
		return source != null ? new HashSet<T>(source, comparer) : new HashSet<T>(comparer);
	}
}