using System.Collections.Generic;
using System.Linq;
using Linqor;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class OrderedExtensionsTests
    {
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new int[] { 0 }, new int[] { 0 })]
        [TestCase(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 })]
        [TestCase(new int[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new int[] { 0, 1, 2, 3, 4 })]
        public void Should_OrderedDistinct(IEnumerable<int> source, IEnumerable<int> expected)
        {
            Assert.That(source.Distinct(), Is.EqualTo(expected));
        }

        [TestCase(new int[] { }, new int[] { }, new int[] { })]
        [TestCase(new int[] { 0 }, new int[] { 0 }, new int[] { 0, 0 })]
        [TestCase(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 0, 1, 1, 2, 2 })]
        [TestCase(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new int[] { 0, 1, 2, 2, 3, 4 })]
        public void Should_OrderedConcat(IEnumerable<int> left, IEnumerable<int> right, IEnumerable<int> expected)
        {
            Assert.That(left.OrderedConcat(right), Is.EqualTo(expected));
        }

        [Test]
        public void Should_OrderedGroupBy()
        {
            int[] source = { 1, 2, 2, 3, 3, 3 };

            IGrouping<int, int>[] actual = source.OrderedGroupBy(n => n).ToArray();

            Assert.That(actual[0].Key, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(new[] { 1 }));

            Assert.That(actual[1].Key, Is.EqualTo(2));
            Assert.That(actual[1], Is.EqualTo(new[] { 2, 2 }));

            Assert.That(actual[2].Key, Is.EqualTo(3));
            Assert.That(actual[2], Is.EqualTo(new[] { 3, 3, 3 }));
        }

        [Test]
        public void Should_OrderedJoin()
        {
            var outer = new[]
            {
                new { Id = 0, Name = "Zero" },
                new { Id = 1, Name = "One" },
                new { Id = 2, Name = "Two" },
                new { Id = 3, Name = "Three" }
            };

            var inner = new[]
            {
                new { Id = 1, Text = "First" },
                new { Id = 2, Text = "Second" },
                new { Id = 3, Text = "Third" },
                new { Id = 4, Text = "Fourth" }
            };

            var actual = outer.OrderedJoin(inner, o => o.Id, i => i.Id, (o, i) => new { o.Id, o.Name, i.Text }).ToArray();

            Assert.That(actual[0], Is.EqualTo(new { Id = 1, Name = "One", Text = "First" }));
            Assert.That(actual[1], Is.EqualTo(new { Id = 2, Name = "Two", Text = "Second" }));
            Assert.That(actual[2], Is.EqualTo(new { Id = 3, Name = "Three", Text = "Third" }));
        }

        [TestCaseSource("GetOrderedUnionTestData")]
        public void Should_OrderedUnion(string description, int[] first, int[] second, int[] expected)
        {
            // Act
            int[] result = first.OrderedUnion(second).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private IEnumerable<ITestCaseData> GetOrderedUnionTestData()
        {
            yield return new TestCaseData("Empty + Empty", new int[] { }, new int[] { }, new int[] { });
            yield return new TestCaseData("Empty + 12345", new int[] { }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 });
            yield return new TestCaseData("12345 + Empty", new int[] { 1, 2, 3, 4, 5 }, new int[] { }, new int[] { 1, 2, 3, 4, 5 });
            yield return new TestCaseData("12345 + 567", new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 });
            yield return new TestCaseData("123 + 34567", new int[] { 1, 2, 3 }, new int[] { 3, 4, 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 });
            yield return new TestCaseData("2468 + 13579", new int[] { 2, 4, 6, 8 }, new int[] { 1, 3, 5, 7, 9 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            yield return new TestCaseData("Unordered", new int[] { 1, 9, 7, 3, 5 }, new int[] { 6, 4, 2, 8 }, new int[] { 1, 6, 4, 2, 8, 9, 7, 3, 5 });
            yield return new TestCaseData("Descending", new int[] { 9, 7, 5, 3, 1 }, new int[] { 8, 6, 4, 2 }, new int[] { 8, 6, 4, 2, 9, 7, 5, 3, 1 });
        }

        [TestCaseSource("GetOrderedUnionTestData")]
        public void Should_OrderedUnion_NotComparable_Objects(string description, int[] first, int[] second, int[] expected)
        {
            // Arrange
            IEnumerable<TestSubject> firstObjects = first.Select(id => new TestSubject { Id = id });
            IEnumerable<TestSubject> secondObjects = second.Select(id => new TestSubject { Id = id });

            // Act
            int[] result = firstObjects.OrderedUnion(secondObjects, t => t.Id).Select(t => t.Id).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Should_OrderedUnion_Descending_Enumerables_Wtih_Custom_Comparer()
        {
            // Arrange
            var first = new[] { 9, 7, 5, 3, 1 };
            var second = new[] { 8, 6, 4, 2 };

            // Act
            int[] result = second.OrderedUnion(first, (x, y) => x.CompareTo(y) * -1).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));
        }

        [TestCaseSource("GetOrderedIntersectTestData")]
        public void Should_OrderedIntersect(string description, int[] first, int[] second, int[] expected)
        {
            // Act
            int[] result = first.OrderedIntersect(second).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private IEnumerable<ITestCaseData> GetOrderedIntersectTestData()
        {
            yield return new TestCaseData("Empty + Empty", new int[] { }, new int[] { }, new int[] { });
            yield return new TestCaseData("Empty + 12345", new int[] { }, new int[] { 1, 2, 3, 4, 5 }, new int[] { });
            yield return new TestCaseData("12345 + Empty", new int[] { 1, 2, 3, 4, 5 }, new int[] { }, new int[] { });
            yield return new TestCaseData("12345 + 567", new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 6, 7 }, new int[] { 5 });
            yield return new TestCaseData("123 + 34567", new int[] { 1, 2, 3 }, new int[] { 3, 4, 5, 6, 7 }, new int[] { 3 });
            yield return new TestCaseData("13579 + 2468", new int[] { 1, 3, 5, 7, 9 }, new int[] { 2, 4, 6, 8 }, new int[] { });
            yield return new TestCaseData("12345 + 23456", new int[] { 1, 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5 });
            yield return new TestCaseData("Unordered", new int[] { 5, 1, 4, 2, 3 }, new int[] { 2, 6, 5, 3, 4 }, new int[] { });
            yield return new TestCaseData("Descending", new int[] { 5, 4, 3, 2, 1 }, new int[] { 6, 5, 4, 3, 2 }, new int[] { });
        }

        [TestCaseSource("GetOrderedIntersectTestData")]
        public void Should_OrderedIntersect_NotComparable_Objects(string description, int[] first, int[] second, int[] expected)
        {
            // Arrange
            IEnumerable<TestSubject> firstObjects = first.Select(id => new TestSubject { Id = id });
            IEnumerable<TestSubject> secondObjects = second.Select(id => new TestSubject { Id = id });

            // Act
            int[] result = firstObjects.OrderedIntersect(secondObjects, t => t.Id).Select(t => t.Id).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Should_OrderedIntersect_Descending_Enumerables_Wtih_Custom_Comparer()
        {
            // Arrange
            var first = new[] { 5, 4, 3, 2, 1 };
            var second = new[] { 6, 5, 4, 3, 2 };

            // Act
            int[] result = second.OrderedIntersect(first, (x, y) => x.CompareTo(y) * -1).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 5, 4, 3, 2 }));
        }

        private class TestSubject
        {
            public int Id { get; set; }
        }
    }
}
