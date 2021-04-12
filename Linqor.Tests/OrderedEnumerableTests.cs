using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class OrderedEnumerableTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public IEnumerable<int> GetEnumerator(
            int[] source)
        {
            return source.AsOrderedBy(n => n);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                new[] { 1, 2, 3, 4, 5 },
                new[] { 3, 5, 1, 4, 2 }
            };

            return testCases.Select(c => new TestCaseData(c).Returns(c));
        }
    }
}