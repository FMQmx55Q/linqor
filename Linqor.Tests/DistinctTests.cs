using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class DistinctTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] Distinct(string[] source)
        {
            return Extensions.Distinct(
                source.AsOrderedBy(),
                (l, r) => l.ID().Equals(r.ID())).ToArray();
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
                .Select(c => (c.Item1, c.Item1.Distinct(Helpers.ByID).ToArray()));

            return testCases.Concat(linqTestCases)
                .Select((c, index) => new TestCaseData((object)c.Item1).Returns(c.Item2).SetName($"Distinct {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}
