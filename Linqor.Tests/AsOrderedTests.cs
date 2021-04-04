using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class AsOrderedTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public void AsOrderedBy(
            IEnumerable<int> source,
            Func<int, int> keySelector,
            IComparer<int> keyComparer,
            bool descending)
        {
            var ordered = !descending
                ? keyComparer == null
                    ? source.AsOrderedBy(keySelector)
                    : source.AsOrderedBy(keySelector, keyComparer)
                : keyComparer == null
                    ? source.AsOrderedByDescending(keySelector)
                    : source.AsOrderedByDescending(keySelector, keyComparer);

            Assert.That(ordered.Source, Is.EqualTo(source));
            Assert.That(ordered.KeySelector, Is.EqualTo(keySelector));
            Assert.That(ordered.KeyComparer, keyComparer == null
                ? Is.EqualTo(Comparer<int>.Default)
                : Is.EqualTo(keyComparer));
            Assert.That(ordered.Descending, Is.EqualTo(descending));
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var source = Enumerable.Range(0, 10);
            Func<int, int> keySelector = n => n;
            var keyComparers = new[] { null, new IntComparer() };
            var descendings = new[] { true, false };

            return keyComparers
                .SelectMany(keyComparer => descendings.Select(desc => (keyComparer, desc)))
                .Select((args, index) =>
                    new TestCaseData(source, keySelector, args.keyComparer, args.desc)
                        .SetName($"AsOrdered {index}"));
        }

        [Test]
        public void AsOrderedLikeParent()
        {
            var parent = Enumerable.Range(0, 10).AsOrderedBy(n => n, new IntComparer());
            var source = Enumerable.Range(0, 10);

            var ordered = source.AsOrderedLike(parent);

            Assert.That(ordered.Source, Is.EqualTo(source));
            Assert.That(ordered.KeySelector, Is.EqualTo(parent.KeySelector));
            Assert.That(ordered.KeyComparer, Is.EqualTo(parent.KeyComparer));
            Assert.That(ordered.Descending, Is.EqualTo(parent.Descending));
        }

        [Test]
        public void AsOrderedLikeParentAndKeySelector()
        {
            var parent = Enumerable.Range(0, 10).AsOrderedBy(n => n, new IntComparer());
            var source = Enumerable.Range(0, 10);
            var keySelector = (Func<int, int>)(n => n);

            var ordered = source.AsOrderedLike(keySelector, parent);

            Assert.That(ordered.Source, Is.EqualTo(source));
            Assert.That(ordered.KeySelector, Is.EqualTo(keySelector));
            Assert.That(ordered.KeyComparer, Is.EqualTo(parent.KeyComparer));
            Assert.That(ordered.Descending, Is.EqualTo(parent.Descending));
        }

        [Test]
        public void AsOrderedLikeGrouping()
        {
            OrderedEnumerable<int, string> parent = Enumerable.Range(0, 1).AsOrderedBy(n => n.ToString(), new StringComparer());
            IEnumerable<IGrouping<string, int>> source = new[] { new Grouping<string, int>("0", Enumerable.Range(0, 1)) };

            OrderedEnumerable<OrderedGrouping<string, int>, string> ordered = source.AsOrderedLike(parent);

            //Assert.That(ordered.Source, Is.EqualTo());
            //Assert.That(ordered.KeySelector, Is.EqualTo(group => group.Key));
            Assert.That(ordered.KeyComparer, Is.EqualTo(parent.KeyComparer));
            Assert.That(ordered.Descending, Is.EqualTo(parent.Descending));

            OrderedGrouping<string, int> firstGroup = ordered.First();

            Assert.That(firstGroup.Source, Is.EqualTo(new[] { 0 }));
            Assert.That(firstGroup.KeySelector, Is.EqualTo(parent.KeySelector));
            Assert.That(firstGroup.KeyComparer, Is.EqualTo(parent.KeyComparer));
            Assert.That(firstGroup.Descending, Is.EqualTo(parent.Descending));
        }
    }
}