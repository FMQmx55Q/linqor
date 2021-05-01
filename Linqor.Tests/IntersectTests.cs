using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    [TestFixture]
    public class IntersectTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] Intersect(string[] left, string[] right)
        {
            return Extensions
                .Intersect(
                    left.AsOrderedBy(NumberInString),
                    right)
                .ToArray();
        }
        public static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, new string[] { }, new string[] { }),
                (new[] { "L0", "L1", "L2" }, new string[] { }, new string[] { }),
                (new string[] { }, new[] { "R0", "R1", "R2" }, new string[] { }),
                
                (new[] { "L0" }, new[] { "R0" }, new[] { "L0" }),
                (new[] { "L0", "L1", "L2" }, new[] { "R0", "R1", "R2" }, new[] { "L0", "L1", "L2" }),

                (new[] { "L0", "L1", "L2" }, new[] { "R2", "R3", "R4" }, new[] { "L2" }),
                (new[] { "L2", "L3", "L4" }, new[] { "R0", "R1", "R2" }, new[] { "L2" }),

                (new[] { "L0", "L0", "L1", "L2", "L2" }, new[] { "R0", "R1", "R1", "R2" }, new[] { "L0", "L1", "L2" }),
                (new[] { "L0", "L0", "L1", "L3", "L3" }, new[] { "R1", "R1", "R2", "R2", "R3" }, new[] { "L1", "L3" }),
            
                (new[] { "L1", "L3", "L5", "L7", "L9" }, new[] { "R2", "R4", "R6", "R8" }, new string[] { }),
            };

            var linqTestCases = testCases
                .Select(c => (c.Item1, c.Item2, Enumerable
                    .Intersect(c.Item1, c.Item2, new NumberInStringComparer())
                    .ToArray()));

            return testCases.Concat(linqTestCases)
                .Select((c, index) => new TestCaseData(c.Item1, c.Item2).Returns(c.Item3).SetName($"Intersect {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}
