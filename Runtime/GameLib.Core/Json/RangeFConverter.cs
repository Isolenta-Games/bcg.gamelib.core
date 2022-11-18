using System;
using GameLib.Core.CommonTypes;
using Newtonsoft.Json;

namespace GameLib.Core.Json
{
	public class RangeFConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(RangeF);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			try
			{
				float from;
				float to;

				if (reader.TokenType == JsonToken.StartArray)
				{
					var arrayVal = serializer.Deserialize<float[]>(reader);
					if (!arrayVal.IsValidIndex(0)) throw new FormatException($"Wrong data for Range type in a json, expected Float or [min, max] but found '{reader.Value}'");

					from = arrayVal[0];
					to = arrayVal.IsValidIndex(1) ? arrayVal[1] : from;
				}
				else if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float)
				{
					var parsedVal = Convert.ToSingle(reader.Value);
					from = to = parsedVal;
				}
				else
				{
					throw new FormatException($"Wrong data for Range type in a json, expected Float or [min, max] but found '{reader.Value}'");
				}

				return new RangeF(from, to);
			}
			catch (Exception ex)
			{
				throw new FormatException($"Error parsing Range from '{reader.Value}'", ex);
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var range = (RangeF)value;
			if (range.min == range.max)
			{
				writer.WriteValue(range.min);
			}
			else
			{
				writer.WriteStartArray();
				writer.WriteValue(range.min);
				writer.WriteValue(range.max);
				writer.WriteEndArray();
			}
		}
	}
}
