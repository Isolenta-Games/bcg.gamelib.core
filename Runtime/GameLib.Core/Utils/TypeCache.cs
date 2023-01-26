using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib.Core.Utils
{
	public static class TypeCache<TBaseType>
	{
		// ReSharper disable once StaticMemberInGenericType
		private static Dictionary<string, Type> _map;

		public static IEnumerable<string> Names
		{
			get
			{
				CacheTypes();
				return _map.Keys.AsEnumerable();
			}
		}
		
		public static IEnumerable<Type> CachedTypes
		{
			get
			{
				CacheTypes();
				return _map.Values.AsEnumerable();
			}
		}

		public static Type FirstOrDefault(string shortTypeName)
		{
			CacheTypes();

			return _map.FirstOrDefault(shortTypeName);
		}

		private static void CacheTypes()
		{
			_map ??= Types.EnumerateAll(x => x.IsClass && !x.IsAbstract && TypeOf<TBaseType>.Raw.IsAssignableFrom(x))
				.ToDictionary(x => x.Name, x => x, StringComparer.InvariantCultureIgnoreCase);
		}

		public static TBaseType Instantiate(string shortTypeName, params object[] args)
		{
			var type = FirstOrDefault(shortTypeName);
			if (type == null)
			{
				return default;
			}

			return (TBaseType)Activator.CreateInstance(type, args);
		}
	}
}