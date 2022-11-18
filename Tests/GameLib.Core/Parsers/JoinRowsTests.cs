using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GameLib.Core.Parsers.Exceptions;
using NUnit.Framework;

namespace GameLib.Core.Parsers
{
	[TestFixture]
	public class JoinRowsTests
	{
		IList<string>[] table1 = {
			new [] {"table1-a", "table1-b", "table1-c"},
			new [] {"table1-a-value", "table1-b-value", "table1-c-value"},
		};
		IList<string>[] table2 = {
			new [] {"table2-a", "table2-b", "table2-c"},
			new [] {"table2-a-value", "table2-b-value", "table2-c-value"},
		};

		[Test]
		public void Creation()
		{
			var t1 = new RawTable(table1);
			var t2 = new RawTable(table2);

			var joinedRow = t1.First().Join(t2.First());
			joinedRow.IsEmpty().Should().BeFalse();
			joinedRow.Headers.Should().HaveCount(6);

			joinedRow = RawTableRowExtensions.Join(new[] {t1.First(), t2.First()});
			joinedRow.IsEmpty().Should().BeFalse();
			joinedRow.Headers.Should().HaveCount(6);
			
			Action create = () => t1.First().Join(t1.First());
			create.Should().Throw<RowValueException>();

			create = () => RawTableRowExtensions.Join(new IRawTableRow[0]);
			create.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void RawGetters()
		{
			var t1 = new RawTable(table1);
			var t2 = new RawTable(table2);

			var joinedRow = t1.First().Join(t2.First());

			joinedRow.RawValue("invalid").Should().BeNull();
			joinedRow.RawValue("table1-a").Should().Be("table1-a-value");
			joinedRow.RawValue("table2-b").Should().Be("table2-b-value");
		}

		[Test]
		public void SimpleGetters()
		{
			var t1 = new RawTable(table1);
			var t2 = new RawTable(table2);

			var joinedRow = t1.First().Join(t2.First());

			joinedRow.Invoking(x => x.GetString("invalid")).Should().Throw<RowValueException>();
			joinedRow.GetString("table1-a").Should().Be("table1-a-value");
			joinedRow.GetString("table2-b").Should().Be("table2-b-value");
		}
	}
}