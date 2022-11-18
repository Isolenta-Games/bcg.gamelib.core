using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using FluentAssertions;
using FluentAssertions.Equivalency;
using GameLib.Core.CommonTypes;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GameLib.Core.Json
{
	[Flags]
	enum FlagsEnum
	{
		Flag1 = 1 << 0,
		Flag2 = 1 << 1,
		Flag3 = 1 << 2,
		Flag4 = 1 << 3,
		Flag5 = Flag1 | Flag3,

		Zero = 0,
	}

	class TestValue<T>
	{
		public T Value { get; set; }
		public T[] ValuesArray { get; set; }
		public List<T> ValuesList { get; set; }

		public T EmptyValue { get; set; }
		public T[] EmptyValuesArray { get; set; }
		public List<T> EmptyValuesList { get; set; }

		public TestValue()
		{
		}

		public TestValue(params T[] values)
		{
			Value = values.First();
			ValuesArray = values;
			ValuesList = values.ToList();
		}
	}

//	[JsonConverter(typeof(DefaultAbstractConverter<ITestClass>))]
	interface ITestClass
	{
	}

//	[JsonAbstractRenamed("TestClassOther")]
	class TestClassA : ITestClass
	{
		public int A { get; set; }

		public ITestClass Inner { get; set; }
		public ITestClass[] InnerArray { get; set; }

		public TestClassA()
		{
		}
			
		public TestClassA(params ITestClass[] inner)
		{
			InnerArray = inner;
			Inner = inner?.FirstOrDefault();
		}
	}

	class TestClassB : ITestClass
	{
		public int B { get; set; }
	}

	
	[TestFixture]
	public class ConverterTests
	{
		[Test]
		public void BigIntegerConverter()
		{
			var source = new TestValue<BigInteger>(new BigInteger(65000000), BigInteger.Zero, BigInteger.One, new BigInteger(100500), new BigInteger(-100500));
			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
			settings.Converters.Add(new BigIntegerConverter());

			var json = JsonConvert.SerializeObject(source, settings);
			json.Should().NotBeNull();
			json.Should().NotBeEmpty();

			var result = JsonConvert.DeserializeObject<TestValue<BigInteger>>(json, settings);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(source);
		}

		[Test]
		public void DateTimeConverter()
		{
			var source = new TestValue<DateTime>(new DateTime(2021, 01, 01, 01, 01, 01),
				new DateTime(1970, 02, 03, 04, 05, 06));
			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
			settings.Converters.Add(new DateTimeConverter(CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern));

			var json = JsonConvert.SerializeObject(source, settings);
			json.Should().NotBeNull();
			json.Should().NotBeEmpty();

			var result = JsonConvert.DeserializeObject<TestValue<DateTime>>(json, settings);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(source);
		}

		public class FlagsEnumComparer : IEquivalencyStep
		{
			public bool CanHandle(IEquivalencyValidationContext context,
				IEquivalencyAssertionOptions config)
			{
				return context.Subject is IEnumerable<FlagsEnum>
					&& context.Expectation is IEnumerable<FlagsEnum>;
			}

			public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator
				parent, IEquivalencyAssertionOptions config)
			{
				var customer = (IEnumerable<FlagsEnum>)context.Subject;
				var employee = (IEnumerable<FlagsEnum>)context.Expectation;

				customer.Should().BeEquivalentTo(employee);
				return true;
			}
		}

		[Test]
		public void FlagsConverter()
		{
			var source = new TestValue<FlagsEnum>(0, FlagsEnum.Zero, FlagsEnum.Zero | FlagsEnum.Flag1,
				FlagsEnum.Flag1, FlagsEnum.Flag2, FlagsEnum.Flag1 | FlagsEnum.Flag2, FlagsEnum.Flag5);

			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
			settings.Converters.Add(new FlagsConverter());

			var json = JsonConvert.SerializeObject(source, settings);
			json.Should().NotBeNull();
			json.Should().NotBeEmpty();

			var result = JsonConvert.DeserializeObject<TestValue<FlagsEnum>>(json, settings);

			result.Should().NotBeNull();
			result.Should()
				.BeEquivalentTo(source,
					opts => opts.Using(new FlagsEnumComparer()));
		}

		[Test]
		public void RangeConverter()
		{
			var source = new TestValue<Range>(Range.Zero, new Range(0, 100), new Range(1), new Range(100, -100));
			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
			settings.Converters.Add(new RangeConverter());

			var json = JsonConvert.SerializeObject(source, settings);
			json.Should().NotBeNull();
			json.Should().NotBeEmpty();

			var result = JsonConvert.DeserializeObject<TestValue<Range>>(json, settings);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(source);
		}

		[Test]
		public void RangeFConverter()
		{
			var source = new TestValue<RangeF>(RangeF.Zero, new RangeF(0.5f, 100.5f), new RangeF(1.2f), new RangeF(100.123f, -100.123f));
			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
			settings.Converters.Add(new RangeConverter());

			var json = JsonConvert.SerializeObject(source, settings);
			json.Should().NotBeNull();
			json.Should().NotBeEmpty();

			var result = JsonConvert.DeserializeObject<TestValue<RangeF>>(json, settings);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(source);
		}

		// [Test]
		// public void DefaultAbstractConverter()
		// {
		// 	var source = new TestValue<ITestClass>(new TestClassA() {A = 10}, null, 
		// 		new TestClassB() {B = 20},
		// 		new TestClassA(new TestClassA() {A=1010}, new TestClassB() {B = 1020})
		// 	{
		// 		A = 100500
		// 	});
		// 	var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
		//
		// 	var json = JsonConvert.SerializeObject(source, settings);
		// 	json.Should().NotBeNull();
		// 	json.Should().NotBeEmpty();
		//
		// 	var result = JsonConvert.DeserializeObject<TestValue<ITestClass>>(json, settings);
		//
		// 	result.Should().NotBeNull();
		// 	result.Should().BeEquivalentTo(source);
		// }
		//
// 		[Test]
// 		public void DefaultAbstractConverterRenamed()
// 		{
// 			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
//
// 			var source = new TestClassA() {A = 10};
//
// 			var json = 
// @"
// {
// 	""t"": ""GameLib.Core.Json.TestClassOther"",
// 	""v"": 
// 	{
// 		""A"": 10
// 	}
// }
// ";
//
// 			var result = JsonConvert.DeserializeObject<ITestClass>(json, settings);
// 			
// 			result.Should().NotBeNull();
// 			result.Should().BeEquivalentTo(source);
// 		}
//
// 		[Test]
// 		public void DefaultAbstractConverterDeleted()
// 		{
// 			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};
//
// 			var json = 
// 				@"
// {
// 	""t"": ""GameLib.Core.Json.SomeRemovedClass"",
// 	""v"": 
// 	{
// 		""A"": 10
// 	}
// }
// ";
//
// 			var result = JsonConvert.DeserializeObject<ITestClass>(json, settings);
// 			
// 			result.Should().BeNull();
// 		}
		
		[Test]
		public void IdConverterTest()
		{
			var source = new TestValue<TestId>(new TestId("Test1"), new TestId("Test2"), TestId.Empty);
			var settings = new JsonSerializerSettings() {Formatting = Formatting.Indented};

			var json = JsonConvert.SerializeObject(source, settings);
			json.Should().NotBeNull();
			json.Should().NotBeEmpty();

			var result = JsonConvert.DeserializeObject<TestValue<TestId>>(json, settings);

			result.Should().NotBeNull();
			result.Should().BeEquivalentTo(source);
		}
	}
}