using System;
using GameLib.Core.Parsers.Base;

namespace GameLib.Core.Json
{
	/// <summary>
	/// serialize class with custom json serialization
	/// GUID: string with uid which never changes
	/// </summary>
	public class JsonSerializeAttribute : Attribute
	{
		public string Guid { get; }

		public JsonSerializeAttribute(string guid)
		{
			Guid = $"#{Base64Encoder.ToBase64String(new Guid(guid).ToByteArray())}";
		}
	}
}