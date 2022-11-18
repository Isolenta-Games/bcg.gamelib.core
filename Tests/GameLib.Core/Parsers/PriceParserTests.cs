using FluentAssertions;
using NUnit.Framework;

namespace GameLib.Core.Parsers
{
	[TestFixture]
	public class PriceParserTests
	{
		[Test]
		public void MakeFakePrice()
		{
			PriceParser.MakeFakePrice("100", 2).Should().Be("200,00");
			PriceParser.MakeFakePrice("100 RUB", 2).Should().Be("200,00 RUB");
			PriceParser.MakeFakePrice("$100", 2).Should().Be("$200,00");
			PriceParser.MakeFakePrice("$ 100 RUB", 2).Should().Be("$ 200,00 RUB");
			PriceParser.MakeFakePrice("100RUB", 2).Should().Be("200,00RUB");
			
			PriceParser.MakeFakePrice("0.50", 2).Should().Be("1,00");
			PriceParser.MakeFakePrice("1 000,30", 2).Should().Be("2\u00A0000,60");
			PriceParser.MakeFakePrice("1,000.30", 2).Should().Be("2\u00A0000,60");
			
			PriceParser.MakeFakePrice("100500", 2).Should().Be("201\u00A0000,00");

		}
	}
}