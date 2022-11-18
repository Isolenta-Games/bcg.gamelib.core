using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameLib.Core.Parsers.Exceptions;
using GameLib.Core.Parsers.Internal;

namespace GameLib.Core.Parsers
{
	/// <summary>
	/// simple table - wrapper for string[][] array
	/// first row used as header and provided column-value mapping like Dictionary
	/// </summary>
	public class RawTable : IEnumerable<IRawTableRow>
	{
		/// <summary>
		/// options for table
		/// </summary>
		[Flags]
		public enum Options
		{
			/// <summary>
			/// no special settings
			/// </summary>
			None = 0,
			
			/// <summary>
			/// remove empty rows
			/// </summary>
			RemoveEmptyRows = 1 << 0,
			
			/// <summary>
			/// default settings
			/// </summary>
			Default = None,
		}
		
		public string Name { get; }

		public List<string> FullNames { get; private set; } = new List<string>(6);

		private readonly Dictionary<string, int> _header = new Dictionary<string, int>(32, StringComparer.InvariantCultureIgnoreCase);

		public class RowData
		{
			public string Location { get;  }
			public IList<string> Values { get; }

			public RowData(string location, IList<string> values)
			{
				Location = location;
				Values = values;
			}
		}
		
		private readonly List<RowData> _rows = new List<RowData>(32);

		public int ColumnCount => _header.Count;
		public int RowCount => _rows.Count;

		public IEnumerable<string> Headers => _header.Keys;

		public RawTable(string name, IReadOnlyList<IList<string>> allValues, Options options = Options.Default) 
			: this(name,name, allValues.SelectToArray((x, index) => new RowData(RawTableRowExtensions.FormatLocation(string.Empty, index), x)), options)
		{
			
		}
		
		public RawTable(string name,string fullName, IReadOnlyList<RowData> rows, Options options = Options.Default) 
		{
			Name = name ?? string.Empty;
			FullNames.Add(fullName);
			var headerRow = rows[0].Values;

			for (var index = 0; index < headerRow.Count; index++)
			{
				var col = headerRow[index]?.Trim();
				if (col.IsNullOrEmpty())
				{
					continue;
				}

				if (_header.ContainsKey(col))
				{
					throw new Exception($"Another column with name '{col}' already exists (table '{name}')");
					
				}
				_header.Add(col, index);
			}

			for (var index = 1; index < rows.Count; index++)
			{
				var row = rows[index];
				
				if (options.Has(Options.RemoveEmptyRows) && row.Values.All(x => x == null || x.ToString().Trim().IsNullOrEmpty()))
				{
					continue;
				}

				_rows.Add(row);
			}
		}

		public RawTable(IReadOnlyList<IList<string>> allValues, Options options = Options.Default): this(string.Empty, allValues, options)
		{
		}

		public int GetRowIndex(IRawTableRow row)
		{
			if (row is RawTableRow tableRow)
			{
				return tableRow.Index;
			}
			else
			{
				throw new RowValueException($"Row is not a RawTableRow!");
			}
		}

		public string GetHeaderByIndex(int index)
		{
			foreach (var data in _header)
			{
				if (data.Value == index)
				{
					return data.Key;
				}
			}
			
			return string.Empty;
		}
		
		public bool HasColumnInHeader(string columnName)
		{
			if (columnName.IsNullOrEmpty())
			{
				return false;
			}

			return _header.ContainsKey(columnName);
		}

		public IRawTableRow GetRow(int rowIndex)
		{
			if (!_rows.IsValidIndex(rowIndex))
			{
				throw new RowValueException($"Row index '{rowIndex}' must be in range [0..{_rows.Count - 1}]");
			}
			
			var row = _rows[rowIndex];
			return new RawTableRow(rowIndex, row.Location, _header, row.Values);
		}

		public IEnumerator<IRawTableRow> GetEnumerator()
		{
			return new RowEnumerator(_header, _rows);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// append table with same layout
		/// </summary>
		public void Append(RawTable other)
		{
			var header1 = _header.OrderBy(x => x.Value).ToArray();
			var header2 = other._header.OrderBy(x => x.Value).ToArray();

			if (!header1.SequenceEqual(header2))
			{
				throw new Exception($"Cannot append table '{other.Name}' to table '{Name}': different headers:\n"
				+ $"{Name}: {header1.JoinToString()}\n"
				+ $"{other.Name}: {header2.JoinToString()}"
				);
			}

			FullNames.Add(other.Name);
			_rows.AddRange(other._rows);
		}

		private class RowEnumerator : IEnumerator<RawTableRow>
		{
			private readonly List<RowData> _values;
			private readonly Dictionary<string, int> _header;
			private int _position;
			private int _rowIndex;

			public RowEnumerator(Dictionary<string, int> header, List<RowData> values)
			{
				_values = values;
				_header = header;
				_position = -1;
			}

			public bool MoveNext()
			{
				_position++;
				return (_position < _values.Count);
			}

			public void Reset()
			{
				_position = -1;
			}

			object IEnumerator.Current => Current;

			public RawTableRow Current
			{
				get
				{
					if (!_values.IsValidIndex(_position))
					{
						throw new IndexOutOfRangeException($"position={_position} mut be in [0..{_values.Count - 1}]");
					}
					
					var row = _values[_position];
					return new RawTableRow(_position, row.Location, _header, row.Values);
				}
			}

			public void Dispose()
			{
			}
		}
	}
}