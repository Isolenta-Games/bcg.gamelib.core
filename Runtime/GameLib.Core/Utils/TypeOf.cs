using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GameLib.Core.Utils
{
	[SuppressMessage("ReSharper", "CheckNamespace"), SuppressMessage("ReSharper", "StaticMemberInGenericType")]
	public static class TypeOf<T>
	{
		public static readonly Type Raw = typeof(T);

		public static readonly string Name = Raw.Name;
		public static readonly TypeCode TypeCode = Type.GetTypeCode(Raw);
		public static readonly Assembly Assembly = Raw.Assembly;
		public static readonly bool IsValueType = Raw.IsValueType;

		public static bool IsAssignableFrom(Type other)
		{
			return Raw.IsAssignableFrom(other);
		}
	}
}