using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    [TestFixture]
    public class ConcatTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] Concat(string[] left, string[] right)
        {
            return Extensions
                .Concat(
                    left.AsOrderedBy(NumberInString),
                    right.AsOrderedBy(NumberInString))
                .ToArray();
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, new string[] { }, new string[] { }),
                (new[] { "L0", "L1", "L2" }, new string[] { }, new[] { "L0", "L1", "L2" }),
                (new string[] { }, new[] { "R0", "R1", "R2" }, new[] { "R0", "R1", "R2" }),

                (new[] { "L0" }, new[] { "R0" }, new[] { "L0", "R0" }),
                (new[] { "L0", "L1", "L2" }, new[] { "R0", "R1", "R2" }, new[] { "L0", "R0", "L1", "R1", "L2", "R2" }),

                (new[] { "L0", "L1", "L2" }, new[] { "R2", "R3", "R4" }, new[] { "L0", "L1", "L2", "R2", "R3", "R4" }),
                (new[] { "L2", "L3", "L4" }, new[] { "R0", "R1", "R2" }, new[] { "R0", "R1", "L2", "R2", "L3", "L4" }),

                (new[] { "L0", "L0", "L1", "L2", "L2" }, new[] { "R0", "R1", "R1", "R2" }, new[] { "L0", "L0", "R0", "L1", "R1", "R1", "L2", "L2", "R2" }),
                (new[] { "L0", "L0", "L1", "L3", "L3" }, new[] { "R1", "R1", "R2", "R2", "R3" }, new[] { "L0", "L0", "L1", "R1", "R1", "R2", "R2", "L3", "L3", "R3" }),

                (new[] { "L2", "L4", "L6", "L8" }, new[] { "R1", "R3", "R5", "R7", "R9" }, new[] { "R1", "L2", "R3", "L4", "R5", "L6", "R7", "L8", "R9" }),
            };

            var linqTestCases = testCases
                .Select(c => (c.Item1, c.Item2, Enumerable
                    .Concat(c.Item1, c.Item2)
                    .OrderBy(NumberInString)
                    .ToArray()));

            return testCases.Concat(linqTestCases)
                .Select((c, index) => new TestCaseData(c.Item1, c.Item2).Returns(c.Item3).SetName($"Concat {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}
