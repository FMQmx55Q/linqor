using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class OrderedEnumerableTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public IOrderedEnumerable<string> ThenBy(
            string[] source,
            bool descending)
        {
            IOrderedEnumerable<string> ordered = source.AsOrderedBy(FirstNumber);

            ordered = !descending
                ? ordered.ThenBy(SecondNumber)
                : ordered.ThenByDescending(SecondNumber);

            return ordered;
        }

        static int FirstNumber(string s) => int.Parse(s.Substring(0, 1));
        static int SecondNumber(string s) => int.Parse(s.Substring(1, 1));

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var ascendingTestCases = new[]
            {
                (
                    new[] { "13", "11", "23", "21", "22", "31" }, new[] { "11", "13", "21", "22", "23", "31" }
                )
            };

            var descendingTestCases = new[]
            {
                (
                    new[] { "11", "13", "22", "23", "21", "31" }, new[] { "13", "11", "23", "22", "21", "31" }
                )
            };

            return Enumerable.Concat(
                ascendingTestCases.Select(c => new TestCaseData(c.Item1, false).Returns(c.Item2)),
                descendingTestCases.Select(c => new TestCaseData(c.Item1, true).Returns(c.Item2)));
        }
    }
}