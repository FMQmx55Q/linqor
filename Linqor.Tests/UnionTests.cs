using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class UnionTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] Union(string[] left, string[] right)
        {
            return Extensions.Union(
                left.AsOrderedBy(),
                right.AsOrderedBy(),
                (l, r) => l.ID().CompareTo(r.ID())).ToArray();
        }

        public static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, new string[] { }, new string[] { }),
                (new[] { "L0", "L1", "L2" }, new string[] { }, new string[] { "L0", "L1", "L2" }),
                (new string[] { }, new[] { "R0", "R1", "R2" }, new string[] { "R0", "R1", "R2" }),

                (new[] { "L0" }, new[] { "R0" }, new string[] { "L0" }),
                (new[] { "L0", "L1", "L2" }, new[] { "R0", "R1", "R2" }, new string[] { "L0", "L1", "L2" }),

                (new[] { "L0", "L1", "L2" }, new[] { "R2", "R3", "R4" }, new string[] { "L0", "L1", "L2", "R3", "R4" }),
                (new[] { "L2", "L3", "L4" }, new[] { "R0", "R1", "R2" }, new string[] { "R0", "R1", "L2", "L3", "L4" }),

                (new[] { "L0", "L0", "L1", "L2", "L2" }, new[] { "R0", "R1", "R1", "R2" }, new string[] { "L0", "L1", "L2" }),
                (new[] { "L0", "L0", "L1", "L3", "L3" }, new[] { "R1", "R1", "R2", "R2", "R3" }, new string[] { "L0", "L1", "R2", "L3" }),
            
                (new[] { "L2", "L4", "L6", "L8" }, new[] { "R1", "R3", "R5", "R7", "R9" }, new string[] { "R1", "L2", "R3", "L4", "R5", "L6", "R7", "L8", "R9" }),
            };

            var linqTestCases = testCases
                .Select(c => (c.Item1, c.Item2, c.Item1.Union(c.Item2, Helpers.ByID).OrderBy(Helpers.ID).ToArray()));

            return testCases.Concat(linqTestCases)
                .Select((c, index) => new TestCaseData(c.Item1, c.Item2).Returns(c.Item3).SetName($"Union {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}
