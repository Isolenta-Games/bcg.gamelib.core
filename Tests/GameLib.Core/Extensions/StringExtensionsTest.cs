using System;
using FluentAssertions;
using NUnit.Framework;

namespace GameLib.Core.Extensions
{
	[TestFixture]
	public class StringExtensionsTest
	{
		[Test]
		public void IsNullOrEmpty()
		{
			((string)null).IsNullOrEmpty().Should().BeTrue();
			"".IsNullOrEmpty().Should().BeTrue();
			
			" ".IsNullOrEmpty().Should().BeFalse();
			"123".IsNullOrEmpty().Should().BeFalse();
		}
		
		[Test]
		public void Take()
		{
			"".Take(0).Should().BeEmpty();
			"".Take(32).Should().BeEmpty();
			
			((string)null).Take(0).Should().BeEmpty();
			((string)null).Take(32).Should().BeEmpty();

			var str = "test string";
			
			str.Take(0).Should().BeEmpty();
			str.Take(4).Should().Be("test");
			str.Take(32).Should().Be("test string");
		}
		
		[Test]
		public void ClampWithEllipsis()
		{
			((string)null).ClampWithEllipsis(32).Should().BeEmpty();
			"".ClampWithEllipsis(32).Should().BeEmpty();
			
			var str = "test string";
			str.ClampWithEllipsis(0).Should().BeEmpty();
			str.ClampWithEllipsis(2).Should().Be("te");
			str.ClampWithEllipsis(3).Should().Be("tes");
			str.ClampWithEllipsis(4).Should().Be("t...");
			str.ClampWithEllipsis(8).Should().Be("test ...");
			str.ClampWithEllipsis(32).Should().Be("test string");
		}
		
		[Test]
		public void Replace()
		{
			((string)null).Replace("Test1", "Test2", StringComparison.InvariantCultureIgnoreCase).Should().BeEmpty();
			"".Replace("Test1", "Test2", StringComparison.InvariantCultureIgnoreCase).Should().BeEmpty();
			
			var str = "test1 string test1";
			str.Replace("Test1", "Test2", StringComparison.InvariantCultureIgnoreCase).Should().Be("Test2 string Test2");
			str.Replace(null, "Test2", StringComparison.InvariantCultureIgnoreCase).Should().Be("test1 string test1");
			str.Replace("Test1", null, StringComparison.InvariantCultureIgnoreCase).Should().Be(" string ");
			
			str.Replace("Test1", "Test2", StringComparison.InvariantCulture).Should().Be("test1 string test1");
			str.Replace(null, "Test2", StringComparison.InvariantCulture).Should().Be("test1 string test1");
			str.Replace("Test1", null, StringComparison.InvariantCulture).Should().Be("test1 string test1");
		}
		
		[Test]
		public void ReplaceAt()
		{
			((string)null).Invoking(x => x.ReplaceAt(10, ' ')).Should().Throw<ArgumentNullException>();
			"".Invoking(x => x.ReplaceAt(10, ' ')).Should().Throw<IndexOutOfRangeException>();
			
			var str = "test string";
			str.ReplaceAt(0, 'a').Should().Be("aest string");
			str.ReplaceAt(5, 'a').Should().Be("test atring");
			str.Invoking(x => x.ReplaceAt(20, ' ')).Should().Throw<IndexOutOfRangeException>();
		}
	}
}