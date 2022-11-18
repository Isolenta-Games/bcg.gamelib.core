using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib.Core.Parsers.Internal
{
	public class FilteredRow : IRawTableRow
	{
		private readonly IRawTableRow _row;
		private readonly List<string> _headers = new List<string>(8);
		private readonly Dictionary<string, string> _columnRemap = new Dictionary<string, string>(8, StringComparer.InvariantCultureIgnoreCase);

		public string Location => _row.Location;
		public IEnumerable<string> Headers => _headers;

		public FilteredRow(IRawTableRow row, string prefix, bool removePrefix)
		{
			_row = row;

			var prefixLen = prefix.Length;
			foreach (var oldName in _row.Headers.Where(x => x.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase)))
			{
				var newName = removePrefix ? oldName.Substring(prefixLen) : oldName;
				_columnRemap.Add(newName, oldName);
				_headers.Add(newName);
			}
		}

		public bool HasColumnInHeader(string columnName)
		{
			return _columnRemap.ContainsKey(columnName);
		}

		public bool IsEmpty()
		{
			return _columnRemap.Values.All(x => _row.IsEmpty(x));
		}

		public bool IsEmpty(string columnName)
		{
			return !_columnRemap.TryGetValue(columnName, out var realName) || _row.IsEmpty(realName);
		}

		public string RawValue(string columnName)
		{
			return _columnRemap.TryGetValue(columnName, out var realName) ? _row.RawValue(realName) : null;
		}

		public bool RawValue(string columnName, out string value)
		{
			if (!_columnRemap.TryGetValue(columnName, out var realName))
			{
				value = null;
				return false;
			}
			
			return _row.RawValue(realName, out value);
		}

		public Exception MakeError(string message)
		{
			return _row.MakeError(message);
		}
	}
}