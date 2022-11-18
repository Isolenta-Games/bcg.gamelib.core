using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using GameLib.Core.Utils;
using Unity.IL2CPP.CompilerServices;

[SuppressMessage("ReSharper", "UnusedType.Global"), SuppressMessage("ReSharper", "CheckNamespace")]
public static class Enums
{
	/// <summary>
	/// convert enum to integer value 
	/// </summary>
	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe TValue ToIntegral<TEnum, TValue>(TEnum value)
		where TEnum : unmanaged, Enum
		where TValue : unmanaged
	{
		if (sizeof(TValue) > sizeof(TEnum))
		{
			TValue o = default;
			*((TEnum*)&o) = value;
			return o;
		}
		else
		{
			return *(TValue*)&value;
		}
	}

	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe TEnum ToEnum<TEnum, TValue>(TValue value)
		where TEnum : unmanaged, Enum
		where TValue : unmanaged
	{
		if (sizeof(TEnum) > sizeof(TValue))
		{
			TEnum o = default;
			*((TValue*)&o) = value;
			return o;
		}
		else
		{
			return *(TEnum*)&value;
		}
	}

	static class EnumCount<T> where T : unmanaged, Enum
	{
		// ReSharper disable once StaticMemberInGenericType
		static int? _count;

		[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public static int Count
		{
			get
			{
				if (!_count.HasValue)
				{
					_count = Enum.GetNames(TypeOf<T>.Raw).Length;
				}

				return _count.Value;
			}
		}
	}

	/// <summary>
	/// return count items of an enum
	/// </summary>
	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Count<T>() where T : unmanaged, Enum
	{
		return EnumCount<T>.Count;
	}

	/// <summary>
	/// return all items of an enum
	/// </summary>
	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T[] Values<T>() where T : unmanaged, Enum
	{
		if (!TypeOf<T>.Raw.IsEnum)
		{
			throw new ArgumentException($"T must be an enumerated type but found {TypeOf<T>.Raw.Name}");
		}

		return (T[])Enum.GetValues(TypeOf<T>.Raw);
	}

	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ToEnum<T>(string str, bool ignoreCase = true) where T : unmanaged, Enum
	{
		T result;
		try
		{
			result = (T)Enum.Parse(TypeOf<T>.Raw, str, ignoreCase);
			if (!Enum.IsDefined(TypeOf<T>.Raw, result))
			{
				throw new Exception();
			}
		}
		catch (Exception)
		{
			throw new InvalidOperationException($"Unknown enum ({TypeOf<T>.Raw.Name}) value: '{str}'");
		}

		return result;
	}

	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static object ToEnum(Type t, string str, bool ignoreCase = true)
	{
		try
		{
			return Enum.Parse(t, str, ignoreCase);
		}
		catch (Exception)
		{
			throw new InvalidOperationException($"Unknown enum ({t.Name}) value: \'{str}\'");
		}
	}

	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryToEnum<T, TValue>(TValue value, out T result) where T : unmanaged, Enum
		where TValue : unmanaged
	{
		if (!Enum.IsDefined(TypeOf<T>.Raw, value))
		{
			result = default(T);
			return false;
		}

		result = ToEnum<T, TValue>(value);

		return true;
	}

	[Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false), MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsDefined<T>(T value) where T : unmanaged, Enum
	{
		return Enum.IsDefined(TypeOf<T>.Raw, (T)value);
	}
}