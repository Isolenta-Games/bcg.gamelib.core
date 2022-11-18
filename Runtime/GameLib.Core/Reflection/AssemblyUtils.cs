using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameLib.Core.Reflection
{
	public static class AssemblyUtils
	{
		/// <summary>
		/// filter only own assemblies by name
		/// </summary>
		public static readonly Func<Assembly, bool> OwnAssemblies = x => x?.GetCustomAttribute<OwnAssemblyAttribute>() != null;

		/// <summary>
		/// get all types from all dependend assemblies
		/// </summary>
		public static IEnumerable<Type> GetAllTypes(this AppDomain domain, Func<Type, bool> typeFilter, Func<Assembly, bool> assemblyFilter = null)
		{
			RecursiveLoadAssemblies(domain, Assembly.GetEntryAssembly(), assemblyFilter);

			var q = domain.GetAssemblies().AsEnumerable();
			if (assemblyFilter != null) q = q.Where(assemblyFilter);

			if (typeFilter == null) return q.SelectMany(s => s.GetTypes());
			else return q.SelectMany(s => s.GetTypes().Where(typeFilter));
		}

		/// <summary>
		/// get all public non-abstract classes from all dependend assemblies
		/// </summary>
		public static IEnumerable<Type> GetDerivedClasses<T>(this AppDomain domain, bool publicOnly, Func<Assembly, bool> assemblyFilter = null)
			where T : class
		{
			RecursiveLoadAssemblies(domain, Assembly.GetEntryAssembly(), assemblyFilter);

			var q = domain.GetAssemblies().AsEnumerable();
			if (assemblyFilter != null) q = q.Where(assemblyFilter);

			var type = typeof(T);
			return q.SelectMany(s => s.GetTypes())
					.Where(p => type.IsAssignableFrom(p) && p.IsClass && (!publicOnly || p.IsPublic) && !p.IsAbstract);
		}

		public static void RecursiveLoadAssemblies(this AppDomain domain, Assembly assembly, Func<Assembly, bool> assemblyFilter)
		{
			if (assemblyFilter != null && !assemblyFilter(assembly)) return;

			foreach (var referencedAssemblyName in assembly.GetReferencedAssemblies())
			{
				var referencedAssembly = domain.Load(referencedAssemblyName);
				RecursiveLoadAssemblies(domain, referencedAssembly, assemblyFilter);
			}
		}

	}
}
