using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib.Core.Parsers.Internal
{
	public class ReplaceRow : IRawTableRow
	{
		private readonly IRawTableRow _row;
		private readonly Dictionary<string, string> _values = new Dictionary<string, string>(8, StringComparer.InvariantCultureIgnoreCase);

		public string Location => _row.Location;
		public IEnumerable<string> Headers => _row.Headers;

		public ReplaceRow(IRawTableRow row, Dictionary<string, string> values)
		{
			_row = row;

			foreach (var keyValuePair in values)
			{
				_values.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public bool HasColumnInHeader(string columnName)
		{
			return _row.HasColumnInHeader(columnName);
		}

		public bool IsEmpty()
		{
			return _row.Headers.All(IsEmpty);
		}

		public bool IsEmpty(string columnName)
		{
			var val = RawValue(columnName)?.Trim();
			return val.IsNullOrEmpty();
		}

		public string RawValue(string columnName)
		{
			return _values.TryGetValue(columnName, out var value) ? value : _row.RawValue(columnName);
		}

		public bool RawValue(string columnName, out string value)
		{
			if ( _values.TryGetValue(columnName, out value))
			{
				return true;
			}
			
			return _row.RawValue(columnName, out value);
		}

		public Exception MakeError(string message)
		{
			return _row.MakeError(message);
		}
	}
}