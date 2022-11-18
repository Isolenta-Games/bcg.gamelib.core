using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GameLib.Core.Parsers.Exceptions;
using NUnit.Framework;

namespace GameLib.Core.Parsers
{
	[TestFixture]
	public class KeyValueTableTests
	{
		[Test]
		public void EmptyTable()
		{
			IList<string>[] table = {
				new [] {"Key", "Value", "Test", null}
			};

			var kt = new RawTable(table).ToKeyValue("Key", "Value");
			kt.IsEmpty().Should().BeTrue();
			kt.Headers.Should().HaveCount(1);
		}
		
		[Test]
		public void FromTableCreation()
		{
			IList<string>[] emptyTable = {
				new [] {"Key", "Value", null}
			};

			IList<string>[] normalTable = {
				new [] {"Key", "Value", "Other", null},
				new [] {"Key1", "Value1", "", null}
			};
			
			Action construct;

			new RawTable(emptyTable).ToKeyValue("Key", "Value");
			
			construct = () => new RawTable(emptyTable).ToKeyValue("Key1", "Value");
			construct.Should().Throw<RowValueException>();

			construct = () => new RawTable(normalTable).ToKeyValue("Key1", "Value");
			construct.Should().Throw<RowValueException>();

			construct = () => new RawTable(normalTable).ToKeyValue("Key", "Value1");
			construct.Should().Throw<RowValueException>();

			var t= new RawTable(normalTable).ToKeyValue("Key", "Value");
			t.Headers.Should().HaveCount(2);
			t.HasColumnInHeader("Other").Should().BeTrue();
			t.HasColumnInHeader("other").Should().BeTrue();
			t.HasColumnInHeader("Key1").Should().BeTrue();
			t.HasColumnInHeader("key1").Should().BeTrue();

			t.HasColumnInHeader("Key").Should().BeFalse();
			t.HasColumnInHeader("Value").Should().BeFalse();
		}

		[Test]
		public void FromRowsCreation()
		{
			IList<string>[] emptyTable = {
				new [] {"Key", "Value", null}
			};

			IList<string>[] normalTable = {
				new [] {"Key", "Value", "Other", null},
				new [] {"Key1", "Value1", "", null}
			};
			
			var tEmpty = new RawTable(emptyTable).SelectToArray(x => x);
			var tNormal = new RawTable(normalTable).SelectToArray(x => x);

			Action construct;

			tEmpty.ToKeyValue("Key", "Value");
			construct = () => tEmpty.ToKeyValue("Key1", "Value");
			construct.Should().NotThrow<RowValueException>();

			construct = () => tEmpty.ToKeyValue("Key", "Value1");
			construct.Should().NotThrow<RowValueException>();

			construct = () => tNormal.ToKeyValue("Key1", "Value");
			construct.Should().Throw<RowValueException>();

			var t= tNormal.ToKeyValue("Key", "Value");
			t.Headers.Should().HaveCount(2);
			t.HasColumnInHeader("Other").Should().BeTrue();
			t.HasColumnInHeader("other").Should().BeTrue();
			t.HasColumnInHeader("Key1").Should().BeTrue();
			t.HasColumnInHeader("key1").Should().BeTrue();

			t.HasColumnInHeader("Key").Should().BeFalse();
			t.HasColumnInHeader("Value").Should().BeFalse();
		}

		[Test]
		public void DuplicatedKeys()
		{
			IList<string>[] table = {
				new [] {"Key", "Value"},
				new [] {"Key1", "Value1"},
				new [] {"Key2", "Value2"},
				new [] {"Key1", "Value3"},
			};

			Action construct;
			
			construct = () => new RawTable(table).ToKeyValue("Key", "Value");
			construct.Should().Throw<RowValueException>();
		}

		[Test]
		public void IsEmpty()
		{
			IList<string>[] table = {
				new [] {"Key", "Value", "Other"},
				new [] {"Key1", "", ""},
				new [] {"Key2", " ", "" },
				new [] {"Key3", "Value3", "Other3"},
			};

			var row = new RawTable(table).ToKeyValue("Key", "Value");
			row.IsEmpty("Key1").Should().BeTrue();
			row.IsEmpty("Key2").Should().BeTrue();
			row.IsEmpty("Key3").Should().BeFalse();
			
			row.IsEmpty("Other").Should().BeTrue();
			
		}

		[Test]
		public void HeaderStructure()
		{
			IList<string>[] table = {
				new [] {"Key", "Value", "Other"},
				new [] {"Key1", "Value1", "Other1"},
				new [] {null, "Value1", ""},
				new [] {"", "Value1", ""},
				new [] {"Key2", null, ""},
				new [] {"Key3", "", ""},
				new [] {"~Key4", "Value4", ""},
			};

			var kt = new RawTable(table).ToKeyValue("Key", "Value");

			kt.IsEmpty().Should().BeFalse();
			
			kt.HasColumnInHeader("Key1").Should().BeTrue();
			kt.HasColumnInHeader("Key2").Should().BeTrue();
			kt.HasColumnInHeader("Key3").Should().BeTrue();
			kt.HasColumnInHeader("Other").Should().BeTrue();
			
			kt.HasColumnInHeader("").Should().BeFalse();
			kt.HasColumnInHeader(null).Should().BeFalse();
			kt.HasColumnInHeader("Value").Should().BeFalse();
			kt.HasColumnInHeader("~Key4").Should().BeFalse();
			kt.HasColumnInHeader("Key4").Should().BeFalse();
		}

		[Test]
		public void RawGetters()
		{
			IList<string>[] table = {
				new [] {"Key", "Value", "Other"},
				new [] {"Key1", "Value1", "Other1"},
				new [] {"Key2", "Value2", ""},
				new [] {"Key3", "Value3", ""},
			};

			var kt = new RawTable(table).ToKeyValue("Key", "Value");

			kt.IsEmpty().Should().BeFalse();

			for (var i = 1; i < table.Length; i++)
			{
				var srcRow = table[i];
				kt.RawValue(srcRow[0]).Should().Be(srcRow[1], $"key={srcRow[0]}");
			}
			
			kt.RawValue("Key").Should().BeNull();
			kt.Invoking(x => x.GetString("Key")).Should().Throw<RowValueException>();
			
			kt.RawValue("Other").Should().Be("Other1");
		}
		
		[Test]
		public void SimpleGetters()
		{
			IList<string>[] table = {
				new [] {"Key", "Value", "Other"},
				new [] {"Key1", "Value1", "Other1"},
				new [] {"Key2", "1"},
				new [] {"Key3", "1.0"},
				new [] {"Key4", "true"},
			};

			var kt = new RawTable(table).ToKeyValue("Key", "Value");

			kt.IsEmpty().Should().BeFalse();

			kt.Invoking(x => x.GetString("Key")).Should().Throw<RowValueException>();
			
			kt.GetString("Other").Should().Be("Other1");

			kt.GetString("Key1").Should().Be("Value1");
			kt.GetInt("Key2").Should().Be(1);
			kt.GetFloat("Key3").Should().Be(1.0f);
			kt.GetBool("Key4").Should().BeTrue();
		}
	}
}