using System;

namespace GameLib.Core.Reflection
{
	/// <summary>
	/// mark assembly for AssemblyUtils.OwnAssemblies filter
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	public class OwnAssemblyAttribute : Attribute
	{
		public string Tag { get; set; }

		public OwnAssemblyAttribute()
		{

		}
	}
}