using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GameLib.Core.Parsers.Exceptions;
using NUnit.Framework;

namespace GameLib.Core.Parsers
{
	[TestFixture]
	public class SelectColumnsTest
	{
		IList<string>[] table =
		{
			new[]
			{
				"test",
				"item.key",
				"item.value",
				"no.item"
			},
			new[]
			{
				"1",
				"value-key",
				"value-value",
				"2"
			},
			new[]
			{
				"1",
				"",
				"",
				"2"
			},
		};

		[Test]
		public void CreationRemovePrefix()
		{
			var t = new RawTable(table);

			var row1 = t.First().SelectColumns("item.", true);
			var row2 = t.Skip(1).First().SelectColumns("item.", true);
			row1.IsEmpty().Should().BeFalse();
			row1.Headers.Should().HaveCount(2);
			row1.HasColumnInHeader("key").Should().BeTrue();
			row1.HasColumnInHeader("value").Should().BeTrue();
			row1.HasColumnInHeader("item.key").Should().BeFalse();
			row1.HasColumnInHeader("item.value").Should().BeFalse();
			row1.HasColumnInHeader("test").Should().BeFalse();
			row1.HasColumnInHeader("no.item").Should().BeFalse();
			row1.HasColumnInHeader("item").Should().BeFalse();
			row1.HasColumnInHeader("item.").Should().BeFalse();

			row2.IsEmpty().Should().BeTrue();
			row2.Headers.Should().HaveCount(2);
			row2.HasColumnInHeader("key").Should().BeTrue();
			row2.HasColumnInHeader("value").Should().BeTrue();
			row2.HasColumnInHeader("item.key").Should().BeFalse();
			row2.HasColumnInHeader("item.value").Should().BeFalse();
			row2.HasColumnInHeader("test").Should().BeFalse();
			row2.HasColumnInHeader("no.item").Should().BeFalse();
			row2.HasColumnInHeader("item").Should().BeFalse();
			row2.HasColumnInHeader("item.").Should().BeFalse();
		}

		[Test]
		public void CreationNotRemovePrefix()
		{
			var t = new RawTable(table);

			var row1 = t.First().SelectColumns("item.", false);
			var row2 = t.Skip(1).First().SelectColumns("item.", false);
			row1.IsEmpty().Should().BeFalse();
			row1.Headers.Should().HaveCount(2);
			row1.HasColumnInHeader("key").Should().BeFalse();
			row1.HasColumnInHeader("value").Should().BeFalse();
			row1.HasColumnInHeader("item.key").Should().BeTrue();
			row1.HasColumnInHeader("item.value").Should().BeTrue();
			row1.HasColumnInHeader("test").Should().BeFalse();
			row1.HasColumnInHeader("no.item").Should().BeFalse();
			row1.HasColumnInHeader("item").Should().BeFalse();
			row1.HasColumnInHeader("item.").Should().BeFalse();

			row2.IsEmpty().Should().BeTrue();
			row2.Headers.Should().HaveCount(2);
			row2.HasColumnInHeader("key").Should().BeFalse();
			row2.HasColumnInHeader("value").Should().BeFalse();
			row2.HasColumnInHeader("item.key").Should().BeTrue();
			row2.HasColumnInHeader("item.value").Should().BeTrue();
			row2.HasColumnInHeader("test").Should().BeFalse();
			row2.HasColumnInHeader("no.item").Should().BeFalse();
			row2.HasColumnInHeader("item").Should().BeFalse();
			row2.HasColumnInHeader("item.").Should().BeFalse();
		}

		[Test]
		public void RawGetters()
		{
			var t = new RawTable(table);

			var row1 = t.First().SelectColumns("item.", true);

			row1.RawValue("invalid").Should().BeNull();
			row1.RawValue("key").Should().Be("value-key");
			row1.RawValue("value").Should().Be("value-value");
		}

		[Test]
		public void SimpleGetters()
		{
			var t = new RawTable(table);

			var row1 = t.First().SelectColumns("item.", true);

			row1.Invoking(x => x.GetString("invalid")).Should().Throw<RowValueException>();
			row1.GetString("key").Should().Be("value-key");
			row1.GetString("value").Should().Be("value-value");
		}
	}
}