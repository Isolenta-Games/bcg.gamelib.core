using System;
using GameLib.Core.CommonTypes;
using Newtonsoft.Json;
using Range = GameLib.Core.CommonTypes.Range;

namespace GameLib.Core.Json
{
	public class RangeConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Range);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			try
			{
				int from;
				int to;

				if (reader.TokenType == JsonToken.StartArray)
				{
					var arrayVal = serializer.Deserialize<int[]>(reader);
					if (!arrayVal.IsValidIndex(0)) throw new FormatException($"Wrong data for Range type in a json, expected Integer or [min, max] but found '{reader.Value}'");

					from = arrayVal[0];
					to = arrayVal.IsValidIndex(1) ? arrayVal[1] : from;
				}
				else if (reader.TokenType == JsonToken.Integer)
				{
					var parsedVal = Convert.ToInt32(reader.Value);
					from = to = parsedVal;
				}
				else
				{
					throw new FormatException($"Wrong data for Range type in a json, expected Integer or [min, max] but found '{reader.Value}'");
				}

				return new Range(from, to);
			}
			catch (Exception ex)
			{
				throw new FormatException($"Error parsing Range from '{reader.Value}'", ex);
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var range = (Range)value;

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
