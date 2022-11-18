using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace GameLib.Core.Extensions
{
	[TestFixture]
	public class EnumerableExtensionsTest
	{
		[Test]
		public void IsValidIndex_List()
		{
			var empty = new List<int>();
			List<int> nullList = null;
			var collection = new List<int>() {10, 20, 30, 40};
			
			empty.IsValidIndex(0).Should().BeFalse();
			empty.IsValidIndex(1).Should().BeFalse();
			empty.IsValidIndex(-1).Should().BeFalse();

			nullList.IsValidIndex(0).Should().BeFalse();
			nullList.IsValidIndex(1).Should().BeFalse();
			nullList.IsValidIndex(-1).Should().BeFalse();
			
			collection.IsValidIndex(0).Should().BeTrue();
			collection.IsValidIndex(1).Should().BeTrue();
			collection.IsValidIndex(-1).Should().BeFalse(); 
		}
		
		[Test]
		public void IsValidIndex_Array()
		{
			var empty = new int[0];
			int[] nullArray = null;
			var collection = new[] {10, 20, 30, 40};
			
			empty.IsValidIndex(0).Should().BeFalse();
			empty.IsValidIndex(1).Should().BeFalse();
			empty.IsValidIndex(-1).Should().BeFalse();

			 nullArray.IsValidIndex(0).Should().BeFalse();
			 nullArray.IsValidIndex(1).Should().BeFalse();
			 nullArray.IsValidIndex(-1).Should().BeFalse();
			
			 collection.IsValidIndex(0).Should().BeTrue();
			 collection.IsValidIndex(1).Should().BeTrue();
			 collection.IsValidIndex(-1).Should().BeFalse(); 
		}

		[Test]
		public void ClampIndex_List()
		{
			var empty = new List<int>();
			List<int> nullList = null;
			var collection = new List<int>() {10, 20, 30, 40};

			nullList.ClampIndex(0).Should().Be(0);
			nullList.ClampIndex(1).Should().Be(1);

			empty.ClampIndex(0).Should().Be(-1);
			empty.ClampIndex(-1).Should().Be(0);

			collection.ClampIndex(-1).Should().Be(0);
			collection.ClampIndex(0).Should().Be(0);
			collection.ClampIndex(2).Should().Be(2);
			collection.ClampIndex(4).Should().Be(collection.Count - 1);
			collection.ClampIndex(8).Should().Be(collection.Count - 1);
		}

		[Test]
		public void ClampIndex_Array()
		{
			var empty = new int[0];
			int[] nullArray = null;
			var collection = new[] {10, 20, 30, 40};
			
			nullArray.ClampIndex(0).Should().Be(0);
			nullArray.ClampIndex(1).Should().Be(1);

			empty.ClampIndex(0).Should().Be(-1);
			empty.ClampIndex(-1).Should().Be(0);

			collection.ClampIndex(-1).Should().Be(0);
			collection.ClampIndex(0).Should().Be(0);
			collection.ClampIndex(2).Should().Be(2);
			collection.ClampIndex(4).Should().Be(collection.Length - 1);
			collection.ClampIndex(8).Should().Be(collection.Length - 1);
		}

		private class TestClass
		{
			public string Text { get; } = "notinit";
			public int Key { get; }

			public TestClass()
			{
			}
			
			public TestClass(string text, int key = 0)
			{
				Text = text;
				Key = key;
			}
		}
		
		[Test]
		public void GetOrAddValue_Dictionary()
		{
			var dictionary = new Dictionary<int, TestClass>(2)
			{
				{1, new TestClass("Fire")}
			};

			dictionary.GetOrAddValue(1).Should().NotBeNull();
			dictionary.GetOrAddValue(1).Text.Should().Be("Fire");
			
			dictionary.GetOrAddValue(2).Should().NotBeNull();
			dictionary.GetOrAddValue(2).Text.Should().Be("notinit");

			dictionary.GetOrAddValue(3, () => new TestClass("123")).Should().NotBeNull();
			dictionary.GetOrAddValue(3).Text.Should().Be("123");
		}
		
		[Test]
		public void GetOrAddValue_List()
		{
			var dictionary = new List<TestClass>(2)
			{
				new TestClass("Fire", 1)
			};

			dictionary.GetOrAddValue(x => x.Key == 0).Should().NotBeNull();
			dictionary.GetOrAddValue(x => x.Key == 0).Text.Should().Be("notinit");

			dictionary.GetOrAddValue(x => x.Key == 1).Should().NotBeNull();
			dictionary.GetOrAddValue(x => x.Key == 1).Text.Should().Be("Fire");
			
			dictionary.GetOrAddValue(x => x.Key == 2).Should().NotBeNull();
			dictionary.GetOrAddValue(x => x.Key == 2).Text.Should().Be("notinit");

			dictionary.GetOrAddValue(x => x.Key == 3, () => new TestClass("123", 3)).Should().NotBeNull();
			dictionary.GetOrAddValue(x => x.Key == 3).Text.Should().Be("123");
		}

		[Test]
		public void AddOnce_List()
		{
			List<int> nullList = null;
			var empty = new List<int>();
			var intCollection1 = new List<int>() {2,2,3,3};
			var intCollection2 = new List<int>() {10,20};

			nullList.AddOnce(null).Should().BeNull();
			
			empty.AddOnce(null).Should().NotBeNull();
			empty.AddOnce(null).Should().BeEmpty();
			empty.AddOnce(10).Should().NotBeNullOrEmpty();
			empty.AddOnce(10).Count.Should().Be(1);
			
			intCollection1.AddOnce(10).Count.Should().Be(5);
			intCollection1.AddOnce(2).Count.Should().Be(5);
			
			intCollection2.AddOnce(10).Count.Should().Be(2);
			intCollection2.AddOnce(-10).Count.Should().Be(3);

			List<int> nullList2 = null;
			var intCollection3 = new List<int>() {2, 2, 10};
			
			nullList.AddOnce(nullList2).Should().BeNull();
			
			intCollection1.AddOnce(nullList2).Should().NotBeNullOrEmpty();
			intCollection1.AddOnce(intCollection3).Count.Should().Be(5);
			intCollection1.AddOnce(intCollection1).Count.Should().Be(5);
		}
	}
}