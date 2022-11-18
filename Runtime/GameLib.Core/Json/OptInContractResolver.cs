using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using GameLib.Core.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GameLib.Core.Json
{
	// ReSharper disable once UnusedType.Global
	public class OptInContractResolver : CamelCasePropertyNamesContractResolver
	{
		public OptInContractResolver()
		{
			IgnoreSerializableAttribute = false;
#pragma warning disable 618
			DefaultMembersSearchFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
#pragma warning restore 618
		}
		
		private readonly Dictionary<Type, List<MemberInfo>> _members = new Dictionary<Type, List<MemberInfo>>(32);

		protected override List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			if (_members.TryGetValue(objectType, out var result))
			{
				return result;
			}

			result = new List<MemberInfo>(32);
			_members.Add(objectType, result);
			foreach (var member in objectType.GetInstanceFieldsAndProperties().Where(CanSerialize))
			{
				if (member is PropertyInfo {CanWrite: false})
				{
					throw new InvalidOperationException($"Cannot serialize {objectType.FullName}.{member.Name}: property must be writeable (at least private setter)");
				}
				
				result.Add(member);
			}
			
			//Debug.Log(result.Select(x => x.Name).JoinToString());

			return result;
		}

		private static bool CanSerialize(MemberInfo member)
		{
			if (member is FieldInfo {IsInitOnly: true})
			{
				return false;
			}

			return member.HasAttribute<DataMemberAttribute>() 
				|| member.HasAttribute<JsonPropertyAttribute>() 
				|| member.HasAttribute<JsonRequiredAttribute>();
		}
	}

}