using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class OrderedExtensionsInfinityTests
    {
        [Test]
        [Timeout(5000)]
        public void Should_OrderedDistinct_Infinite()
        {
            var source = Generate(0, 3, 1);
            var expected = new[] { 10, 11, 12, 13, 14 };

            var actual = source.OrderedDistinct().Skip(10).Take(5);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [Timeout(5000)]
        public void Should_OrderedConcat_Infinite()
        {
            var left = Generate(0, 1, 1);
            var right = Generate(1, 1, 1);
            var expected = new[] { 0, 1, 1, 2, 2 };

            var actual = left.OrderedConcat(right).Take(5);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [Timeout(5000)]
        public void Should_OrderedGroupBy_Infinite()
        {
            IEnumerable<int> source = Generate(1, 2, 1);

            IGrouping<int, int>[] actual = source.OrderedGroupBy(n => n).Take(3).ToArray();

            Assert.That(actual[0].Key, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(new[] { 1, 1 }));

            Assert.That(actual[1].Key, Is.EqualTo(2));
            Assert.That(actual[1], Is.EqualTo(new[] { 2, 2 }));

            Assert.That(actual[2].Key, Is.EqualTo(3));
            Assert.That(actual[2], Is.EqualTo(new[] { 3, 3 }));
        }

        [Test]
        [Timeout(5000)]
        public void Should_OrderedJoin_Infinite()
        {
            var outer = Generate(0, 1, 1).Select(i => new { Id = i, Name = i.ToString() });
            var inner = Generate(1, 1, 1).Select(i => new { Id = i, Text = i.ToString() });

            var actual = outer.OrderedJoin(inner, o => o.Id, i => i.Id, (o, i) => new { o.Id, o.Name, i.Text }).Take(3).ToArray();

            Assert.That(actual[0], Is.EqualTo(new { Id = 1, Name = "1", Text = "1" }));
            Assert.That(actual[1], Is.EqualTo(new { Id = 2, Name = "2", Text = "2" }));
            Assert.That(actual[2], Is.EqualTo(new { Id = 3, Name = "3", Text = "3" }));
        }

        [Test]
        [Timeout(5000)]
        public void Should_OrderedUnion_Infinite()
        {
            // Arrange
            var first = Generate(1, 1, 2);
            var second = Generate(2, 1, 2);

            // Act
            int[] result = first.OrderedUnion(second).Take(7).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6, 7 }));
        }

        [Test]
        [Timeout(5000)]
        public void Should_OrderedIntersect_Infinite()
        {
            // Arrange
            var first = Generate(1, 1, 2);
            var second = Generate(5, 1, 2);

            // Act
            int[] result = first.OrderedIntersect(second).Take(5).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 5, 7, 9, 11, 13 }));
        }

        private IEnumerable<int> Generate(int start, int count, int step)
        {
            while (true)
            {
                foreach (int value in Enumerable.Repeat(start, count))
                {
                    yield return value;
                }
                start += step;
            }
        }
    }
}
