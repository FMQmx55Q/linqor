using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class GroupByTests
    {
        [TestCase(TestName = "GroupBy { 1, 2, 2, 3, 3, 3 }")]
        public void GroupBy()
        {
            int[] source = { 1, 2, 2, 3, 3, 3 };

            IGrouping<int, int>[] actual = source.OrderedGroupBy(i => i, (i1, i2) => i1.Equals(i2)).ToArray();

            Assert.That(actual[0].Key, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(new[] { 1 }));

            Assert.That(actual[1].Key, Is.EqualTo(2));
            Assert.That(actual[1], Is.EqualTo(new[] { 2, 2 }));

            Assert.That(actual[2].Key, Is.EqualTo(3));
            Assert.That(actual[2], Is.EqualTo(new[] { 3, 3, 3 }));
        }

        [TestCase(TestName = "GroupBy ∞ { 1, 1, 2, 2, 3, 3 }")]
        [Timeout(3000)]
        public void GroupByInfinite()
        {
            IEnumerable<int> source = TestCases.Generate(1, 2, 1);

            IGrouping<int, int>[] actual = source.OrderedGroupBy(n => n, (i1, i2) => i1.Equals(i2)).Take(3).ToArray();

            Assert.That(actual[0].Key, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(new[] { 1, 1 }));

            Assert.That(actual[1].Key, Is.EqualTo(2));
            Assert.That(actual[1], Is.EqualTo(new[] { 2, 2 }));

            Assert.That(actual[2].Key, Is.EqualTo(3));
            Assert.That(actual[2], Is.EqualTo(new[] { 3, 3 }));
        }
    }
}
