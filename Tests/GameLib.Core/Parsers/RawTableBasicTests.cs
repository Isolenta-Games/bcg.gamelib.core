using System;
using System.Collections.Generic;
using FluentAssertions;
using GameLib.Core.Parsers.Exceptions;
using NUnit.Framework;

namespace GameLib.Core.Parsers
{
	[TestFixture]
	public class RawTableBasicTests
	{
		enum TestEnum
		{
			Value1 = 10,
			Value2 = 20
		}
		
		[Test]
		public void EmptyTable()
		{
			IList<string>[] table = {
				new [] {"col1", "col2", "col3", "col4", "col5", "", null}
			};

			var t = new RawTable(table);

			t.ColumnCount.Should().Be(5);
			t.RowCount.Should().Be(0);
		}
		
		[Test]
		public void RowCount()
		{
			IList<string>[] table = {
				new [] {"col1", "col2", "col3", "col4", "col5", "", null},
				new [] {"1", " string ", "3.0", "true", "false"},
				new [] {"2", " string ", null, "true", "false"},
				new [] {"3", " string ", "", "true", "false"},
			};

			var t = new RawTable(table);
			
			t.ColumnCount.Should().Be(5);
			t.RowCount.Should().Be(3);

			t.GetRow(0).Should().NotBeNull();
			t.GetRow(1).Should().NotBeNull();
			t.GetRow(2).Should().NotBeNull();
			t.Invoking(x => x.GetRow(3)).Should().Throw<RowValueException>();
		}

		[Test]
		public void EmptyCells()
		{
			IList<string>[] table = {
				new [] {"col1", "col2", null},
				new [] {"", "", "", ""}
			};

			var t = new RawTable(table);

			t.ColumnCount.Should().Be(2);
			t.RowCount.Should().Be(1);

			var row = t.GetRow(0);
			row.Invoking(x => x.GetString("col1")).Should().Throw<RowValueException>();
			row.Invoking(x => x.GetString("col2")).Should().Throw<RowValueException>();
			row.Invoking(x => x.GetString("col3")).Should().Throw<RowValueException>();
		}

		[Test]
		public void EmptyCellsRemove()
		{
			IList<string>[] emptyTable = {
				new [] {"col1", "col2", null},
				new [] {"", "", "", ""}
			};

			var t = new RawTable(emptyTable, RawTable.Options.RemoveEmptyRows);

			t.ColumnCount.Should().Be(2);
			t.RowCount.Should().Be(0);
		}
		
		
		static readonly IList<string>[] testTable = {
			new [] {"col1", "col2", "col3", "col4", "col5", "col6", "col7", null},
			new [] {"1", " string ", "3.0", "true", "false", "Value1", "31.12.2019"},
			new [] {" 1 ", "string ", "3", "+", "-", " Value1 ", "31/12/2019"},
			new [] {"1   ", " string", " 3.0", "+", "", "value1", "31/12/2019 23:59"},
			new [] {"1", "   string ", "3.00000", "1", "0", "Value1", ""},
			new [] {"1", "\tstring", " 3.0 ", "true", "false", "Value1", "no date"},
			new string [] {},
			new [] {"1", "string", "3,0", "true", "false", "Value1", "    31/12/2019 "},
		};

		private const int EmptyRowIndex = 5;

		[Test]
		public void Dimensions()
		{
			var t = new RawTable(testTable);

			t.ColumnCount.Should().Be(7);
			t.RowCount.Should().Be(7);
		}
		
		[Test]
		public void Iterate()
		{
			var t = new RawTable(testTable);

			var rowCount = 0;
			foreach (var row in t)
			{
				t.GetRowIndex(row).Should().Be(rowCount);
				++rowCount;
			}

			rowCount.Should().Be(7);
		}
		
		[Test]
		public void HasColumnInHeader()
		{
			var t = new RawTable(testTable);

			t.HasColumnInHeader("col1").Should().BeTrue();
			t.HasColumnInHeader("Col1").Should().BeTrue();
			t.HasColumnInHeader("COL1").Should().BeTrue();
			t.HasColumnInHeader("cols1").Should().BeFalse();

			var row0 = t.GetRow(0);
			row0.HasColumnInHeader("col1").Should().BeTrue();
			row0.HasColumnInHeader("Col1").Should().BeTrue();
			row0.HasColumnInHeader("COL1").Should().BeTrue();
			row0.HasColumnInHeader("cols1").Should().BeFalse();
		}

		[Test]
		public void HeaderIndex()
		{
			var t = new RawTable(testTable);

			t.GetHeaderByIndex(0).Should().Be("col1");
			t.GetHeaderByIndex(1).Should().Be("col2");
			t.GetHeaderByIndex(100).Should().BeNullOrEmpty();
		}

		private static void EmptyCellsExceptions<T>(IRawTableRow row, Func<IRawTableRow, string, T> getter)
		{
			row.Invoking(x => getter(x, "col1")).Should().Throw<RowValueException>(row.Location);
			row.Invoking(x => getter(x, "col2")).Should().Throw<RowValueException>(row.Location);
			row.Invoking(x => getter(x, "col3")).Should().Throw<RowValueException>(row.Location);
			row.Invoking(x => getter(x, "col4")).Should().Throw<RowValueException>(row.Location);
			row.Invoking(x => getter(x, "col5")).Should().Throw<RowValueException>(row.Location);
			row.Invoking(x => getter(x, "col6")).Should().Throw<RowValueException>(row.Location);
			row.Invoking(x => getter(x, "col7")).Should().Throw<RowValueException>(row.Location);
		}
		
		[Test]
		public void ValueInt()
		{
			var t = new RawTable(testTable);

			foreach (var row in t)
			{
				if (t.GetRowIndex(row) == EmptyRowIndex)
				{
					EmptyCellsExceptions(row, (r, col) => r.GetInt(col));
					continue;
				}
				
				row.GetInt("col1").Should().Be(1, row.Location);
				row.Invoking(x => x.GetInt("col2")).Should().Throw<RowValueException>(row.Location);
				
				if (t.GetRowIndex(row) == 1)
				{
					row.GetInt("col3").Should().Be(3, row.Location);
				}
				else
				{
					row.Invoking(x => x.GetInt("col3")).Should().Throw<RowValueException>(row.Location);
				}

				if (t.GetRowIndex(row) == 3)
				{
					row.GetInt("col4").Should().Be(1, row.Location);
					row.GetInt("col5").Should().Be(0, row.Location);
				}
				else
				{
					row.Invoking(x => x.GetInt("col4")).Should().Throw<RowValueException>(row.Location);
					row.Invoking(x => x.GetInt("col5")).Should().Throw<RowValueException>(row.Location);
				}
				
				row.Invoking(x => x.GetInt("col6")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetInt("col7")).Should().Throw<RowValueException>(row.Location);
			}
		}
		
		[Test]
		public void ValueString()
		{
			var t = new RawTable(testTable);

			foreach (var row in t)
			{
				if (t.GetRowIndex(row) == EmptyRowIndex)
				{
					EmptyCellsExceptions(row, (r, col) => r.GetString(col));
					continue;
				}
				
				row.GetString("col1").Should().Contain("1", row.Location);


				if (t.GetRowIndex(row) == 6)
				{
					row.GetString("col2").Should().Be("string", row.Location);
				}
				else
				{
					row.GetString("col2").Should().NotBe("string", row.Location);
				}
				
				row.GetString("col2").Should().Contain("string", row.Location);
			}
		}

		[Test]
		public void ValueStringDefault()
		{
			IList<string>[] table = {
				new [] {"col1", "col2"},
				new [] {"", null}
			};

			var t = new RawTable(table);

			var row = t.GetRow(0);

			row.Invoking(x => x.GetString("col1")).Should().Throw<RowValueException>();
			row.Invoking(x => x.GetString("col2")).Should().Throw<RowValueException>();
			row.GetString("col1", "").Should().BeEmpty();
			row.GetString("col2", "").Should().BeEmpty();
			row.GetString("col1", null).Should().BeNull();
			row.GetString("col2", null).Should().BeNull();
		}
		
		[Test]
		public void ValueFloat()
		{
			var t = new RawTable(testTable);

			foreach (var row in t)
			{
				if (t.GetRowIndex(row) == EmptyRowIndex)
				{
					EmptyCellsExceptions(row, (r, col) => r.GetFloat(col));
					continue;
				}
				
				row.GetFloat("col1").Should().Be(1.0f, row.Location);
				row.Invoking(x => x.GetFloat("col2")).Should().Throw<RowValueException>(row.Location);
				
				row.GetFloat("col3").Should().Be(3.0f, row.Location);

				if (t.GetRowIndex(row) == 3)
				{
					row.GetFloat("col4").Should().Be(1.0f, row.Location);
					row.GetFloat("col5").Should().Be(0.0f, row.Location);
				}
				else
				{
					row.Invoking(x => x.GetFloat("col4")).Should().Throw<RowValueException>(row.Location);
					row.Invoking(x => x.GetFloat("col5")).Should().Throw<RowValueException>(row.Location);
				}
				
				row.Invoking(x => x.GetFloat("col6")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetFloat("col7")).Should().Throw<RowValueException>(row.Location);
			}
		}
		
		[Test]
		public void ValueDouble()
		{
			var t = new RawTable(testTable);

			foreach (var row in t)
			{
				if (t.GetRowIndex(row) == EmptyRowIndex)
				{
					EmptyCellsExceptions(row, (r, col) => r.GetDouble(col));
					continue;
				}
				
				row.GetDouble("col1").Should().Be(1.0f, row.Location);
				row.Invoking(x => x.GetDouble("col2")).Should().Throw<RowValueException>(row.Location);
				
				row.GetDouble("col3").Should().Be(3.0f, row.Location);

				if (t.GetRowIndex(row) == 3)
				{
					row.GetDouble("col4").Should().Be(1.0f, row.Location);
					row.GetDouble("col5").Should().Be(0.0f, row.Location);
				}
				else
				{
					row.Invoking(x => x.GetDouble("col4")).Should().Throw<RowValueException>(row.Location);
					row.Invoking(x => x.GetDouble("col5")).Should().Throw<RowValueException>(row.Location);
				}
				
				row.Invoking(x => x.GetDouble("col6")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetDouble("col7")).Should().Throw<RowValueException>(row.Location);
			}
		}
		
		[Test]
		public void ValueBool()
		{
			var t = new RawTable(testTable);

			foreach (var row in t)
			{
				if (t.GetRowIndex(row) == EmptyRowIndex)
				{
					row.GetBool("col1").Should().BeFalse(row.Location);
					row.GetBool("col2").Should().BeFalse(row.Location);
					row.GetBool("col3").Should().BeFalse(row.Location);
					row.GetBool("col4").Should().BeFalse(row.Location);
					row.GetBool("col5").Should().BeFalse(row.Location);
					row.GetBool("col6").Should().BeFalse(row.Location);
					row.GetBool("col7").Should().BeFalse(row.Location);
					continue;
				}
				
				row.GetBool("col1").Should().BeTrue(row.Location);
				row.Invoking(x => x.GetBool("col2")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetBool("col3")).Should().Throw<RowValueException>(row.Location);
				row.GetBool("col4").Should().BeTrue(row.Location);
				row.GetBool("col5").Should().BeFalse(row.Location);
				row.Invoking(x => x.GetBool("col6")).Should().Throw<RowValueException>(row.Location);
				
				if (t.GetRowIndex(row) == 3)
				{
					row.GetBool("col7").Should().BeFalse(row.Location);
				}
				else
				{
					row.Invoking(x => x.GetBool("col7")).Should().Throw<RowValueException>(row.Location);
				}
			}
		}
		
		[Test]
		public void ValueEnum()
		{
			var t = new RawTable(testTable);

			foreach (var row in t)
			{
				if (t.GetRowIndex(row) == EmptyRowIndex)
				{
					EmptyCellsExceptions(row, (r, col) => r.GetEnum<TestEnum>(col));
					continue;
				}
				
				row.Invoking(x => x.GetEnum<TestEnum>("col1")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetEnum<TestEnum>("col2")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetEnum<TestEnum>("col3")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetEnum<TestEnum>("col4")).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetEnum<TestEnum>("col5")).Should().Throw<RowValueException>(row.Location);
				row.GetEnum<TestEnum>("col6").Should().Be(TestEnum.Value1, row.Location);
				row.Invoking(x => x.GetEnum<TestEnum>("col7")).Should().Throw<RowValueException>(row.Location);
			}
		}
		
		[Test]
		public void DateTime()
		{
			var t = new RawTable(testTable);

			var formatDate = "d/MM/yyyy";
			var formatDateTime = "d/MM/yyyy H:mm";

			var date = new DateTime(2019, 12, 31, 00, 00, 00, DateTimeKind.Utc);
			var dateTime = new DateTime(2019, 12, 31, 23, 59, 00, DateTimeKind.Utc);

			foreach (var row in t)
			{
				if (t.GetRowIndex(row) == EmptyRowIndex)
				{
					EmptyCellsExceptions(row, (r, col) => r.GetDateTimeUTC(col, formatDate));
					continue;
				}

				row.Invoking(x => x.GetDateTimeUTC("col1", formatDate)).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetDateTimeUTC("col2", formatDate)).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetDateTimeUTC("col3", formatDate)).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetDateTimeUTC("col4", formatDate)).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetDateTimeUTC("col5", formatDate)).Should().Throw<RowValueException>(row.Location);
				row.Invoking(x => x.GetDateTimeUTC("col6", formatDate)).Should().Throw<RowValueException>(row.Location);
			}

			t.GetRow(0).Invoking(x => x.GetDateTimeUTC("col7", formatDate)).Should().Throw<RowValueException>(t.GetRow(0).Location);
			t.GetRow(1).GetDateTimeUTC("col7", formatDate).Should().Be(date, t.GetRow(1).Location);
			t.GetRow(2).GetDateTimeUTC("col7", formatDateTime).Should().Be(dateTime, t.GetRow(2).Location);
			t.GetRow(3).Invoking(x => x.GetDateTimeUTC("col7", formatDate)).Should().Throw<RowValueException>(t.GetRow(3).Location);
			t.GetRow(4).Invoking(x => x.GetDateTimeUTC("col7", formatDate)).Should().Throw<RowValueException>(t.GetRow(4).Location);
			t.GetRow(5).Invoking(x => x.GetDateTimeUTC("col7", formatDate)).Should().Throw<RowValueException>(t.GetRow(5).Location);
			t.GetRow(6).GetDateTimeUTC("col7", formatDate).Should().Be(date, t.GetRow(5).Location);
		}
	}
}