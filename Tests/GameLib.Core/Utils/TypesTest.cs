using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace GameLib.Core.Utils
{
	[TestFixture]
	public class TypesTest
	{
		[Test]
		public void GetTypeTest()
		{
			Types.GetType(null).Should().BeNull();
			Types.GetType("").Should().BeNull();

			Types.GetType("GameLib.Core.Utils.TypesTest").Should().Be<TypesTest>();
			Types.GetType("TypeOfTestNotExists").Should().BeNull();
		}
		
		[Test]
		public void GetExistingType()
		{
			FluentActions.Invoking(() => Types.GetExistingType(null)).Should().Throw<TypeLoadException>();
			FluentActions.Invoking(() => Types.GetExistingType("")).Should().Throw<TypeLoadException>();

			Types.GetExistingType("GameLib.Core.Utils.TypesTest").Should().Be<TypesTest>();
			FluentActions.Invoking(() => Types.GetExistingType("TypeOfTestNotExists")).Should().Throw<TypeLoadException>();
		}
		
		[Test]
		public void EnumerateAll()
		{
			FluentActions.Invoking(() => Types.EnumerateAll(null)).Should().Throw<ArgumentNullException>();
			var types = Types.EnumerateAll(x => x == typeof(TypesTest)).ToArray();

			types.Should().NotBeEmpty()
				.And.ContainSingle()
				.And.ContainInOrder(typeof(TypesTest))
				;
		}
	}
}