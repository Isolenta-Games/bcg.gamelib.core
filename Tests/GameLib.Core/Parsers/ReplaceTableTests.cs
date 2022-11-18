using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace GameLib.Core.Parsers
{
	[TestFixture]
	public class ReplaceTableTests
	{
		[Test]
		public void ReplaceValue()
		{
			IList<string>[] table = {
				new [] {"c1", "c2", null},
				new [] {"src1", "src2", null}
			};

			var t = new RawTable(table);
			var srcRow = t.First();
			var dstRow = srcRow.RawReplace("c1", "dst");

			srcRow.GetString("c1").Should().Be("src1");
			srcRow.GetString("c2").Should().Be("src2");
			
			dstRow.GetString("c1").Should().Be("dst");
			dstRow.GetString("c2").Should().Be("src2");
		}
		
		[Test]
		public void EmptyCount()
		{
			IList<string>[] table = {
				new [] {"c1", "c2", null},
				new [] {"src1", "src2", null}
			};

			var t = new RawTable(table);
			var srcRow = t.First();
			var dstRow = srcRow.RawReplace("c1", "");

			srcRow.IsEmpty("c1").Should().BeFalse();
			srcRow.IsEmpty("c2").Should().BeFalse();
			srcRow.IsEmpty().Should().BeFalse();
			
			dstRow.IsEmpty("c1").Should().BeTrue();
			dstRow.IsEmpty("c2").Should().BeFalse();
			dstRow.IsEmpty().Should().BeFalse();
		}
		
	}
}