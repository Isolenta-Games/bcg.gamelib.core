using System;
using FluentAssertions;
using NUnit.Framework;

namespace GameLib.Core.Utils
{
	[TestFixture]
	public class TypeOfTest
	{
		[Test]
		public void TypeOfBasics()
		{
			TypeOf<TypeOfTest>.Name.Should().Be(nameof(TypeOfTest));
			TypeOf<TypeOfTest>.Raw.Should().Be<TypeOfTest>();
			TypeOf<TypeOfTest>.IsValueType.Should().BeFalse();
			TypeOf<TypeOfTest>.Assembly.Should().BeSameAs(typeof(TypeOfTest).Assembly);
			TypeOf<TypeOfTest>.TypeCode.Should().Be(Type.GetTypeCode(typeof(TypeOfTest)));
		}
	}
}