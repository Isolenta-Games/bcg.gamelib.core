using Newtonsoft.Json.Converters;

namespace GameLib.Core.Json
{
	public class DateTimeConverter : IsoDateTimeConverter
	{
		public DateTimeConverter(string format)
		{
			DateTimeFormat = format;
		}
	}
}
