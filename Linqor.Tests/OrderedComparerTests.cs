using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    [TestFixture]
    public class OrderedComparerTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public int Compare(
            Func<string, int> keySelector,
            IComparer<int> keyComparer,
            bool descending,
            string x,
            string y)
        {
            var comparer = new OrderedComparer<string, int>(
                keySelector,
                keyComparer,
                descending);

            return comparer.Compare(x, y);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            Func<string, int> keySelector = NumberInString;
            var keyComparer = new IntComparer();

            var ascendingTestCases = new[]
            {
                ("L0", "R0", 0),
                ("L0", "R1", -1),
                ("L1", "R0", 1)
            };

            var descendingTestCases = ascendingTestCases
                .Select(c => (c.Item1, c.Item2, c.Item3 * -1));

            return ascendingTestCases
                .Select(c => new TestCaseData(keySelector, keyComparer, false, c.Item1, c.Item2).Returns(c.Item3))
                .Concat(descendingTestCases
                    .Select(c => new TestCaseData(keySelector, keyComparer, true, c.Item1, c.Item2).Returns(c.Item3)));
        }
    }
}