using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    [TestFixture]
    public class DistinctTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] Distinct(string[] source)
        {
            return Extensions
                .Distinct(source.AsOrderedBy(NumberInString))
                .ToArray();
        }

        public static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, new string[] { }),
                (new[] { "S0" }, new string[] { "S0" }),
                (new[] { "S0", "S1", "S2" }, new string[] { "S0", "S1", "S2" }),
                (new[] { "S0", "S1", "S2", "S2", "S3", "S3", "S3", "S4" }, new string[] { "S0", "S1", "S2", "S3", "S4" })
            };
            
            var linqTestCases = testCases
                .Select(c => (c.Item1, Enumerable
                    .Distinct(c.Item1, new NumberInStringComparer())
                    .ToArray()));

            return testCases.Concat(linqTestCases)
                .Select((c, index) => new TestCaseData((object)c.Item1).Returns(c.Item2).SetName($"Distinct {Helpers.Get2DID(index, testCases.Length)}"));
        }

        [TestCaseSource(nameof(GetErrorCases))]
        public void DistinctError(string[] left)
        {
            var result = Extensions
                .Distinct(left.AsOrderedBy(NumberInString));

            Assert.That(() => result.ToArray(), Throws.TypeOf<UnorderedElementDetectedException>());
        }

        private static IEnumerable<TestCaseData> GetErrorCases()
        {
            var exceptionTestCases = new[]
            {
                new[] { "L2", "L0", "L1" },
                new[] { "L2", "L1", "L1", "L0" },
            };

            return exceptionTestCases
                .Select(c => new TestCaseData((object)c));
        }
    }
}
