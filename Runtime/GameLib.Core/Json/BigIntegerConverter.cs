using System;
using System.Numerics;
using GameLib.Core.Parsers.Base;
using Newtonsoft.Json;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#else 
using System.Diagnostics;
#endif

namespace GameLib.Core.Json
{
	public class BigIntegerConverter : JsonConverter
	{
		private static readonly Type BigIntegerType = typeof(BigInteger);

		private readonly bool _forceToString;

		public BigIntegerConverter()
		{
		}

		public BigIntegerConverter(bool forceToString)
		{
			_forceToString = forceToString;
		}
		
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Debug.Assert(value != null && value.GetType() == BigIntegerType);
			
			var v = (BigInteger)value;

			if (_forceToString)
			{
				writer.WriteValue(v.ToString());
			}
			else
			{
				var result = Base64Encoder.Default.ToBase(v.ToByteArray());
				writer.WriteValue(result);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var str = reader.Value.ToString();
			if (!_forceToString)
			{
				BigInteger result;
				if (!BigInteger.TryParse(str, out result))
				{
					var bytes = Base64Encoder.Default.FromBase(str);
					return new BigInteger(bytes);
				}
				return result;
			}
			else
			{
				return BigInteger.Parse(str);
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == BigIntegerType;
		}
	}
}