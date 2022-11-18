using System;
using Newtonsoft.Json;
using UnityEngine;

namespace GameLib.Core.Json
{
	/// <summary>
	/// ID for database entity
	/// </summary>
	[Serializable, JsonConverter(typeof(GenericIdConverter<TestId>))]
	public struct TestId : IComparable<TestId>
	{
		public static readonly TestId Empty = default;
		
		[SerializeField] private string _id;

		public TestId(string id)
		{
			if (id.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(id));
			}
			
			_id = id;
		}

		public bool Equals(string id)
		{
			return _id == id;
		}

		public bool Equals(TestId other)
		{
			return _id == other._id;
		}

		public bool EqualsIgnoreCase(TestId other)
		{
			return _id.Equals(other._id, StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool Equals(object obj)
		{
			return obj is TestId other && Equals(other);
		}

		public override int GetHashCode()
		{
			return _id != null ? _id.GetHashCode() : 0;
		}

		public int CompareTo(TestId other)
		{
			return string.Compare(_id, other._id, StringComparison.Ordinal);
		}

		public static bool operator ==(TestId a, TestId b)
		{
			return a._id == b._id;
		}

		public static bool operator !=(TestId a, TestId b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			return _id;
		}
	}
}