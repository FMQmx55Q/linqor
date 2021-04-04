using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    [TestFixture]
    public class JoinTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] Join(string[] left, string[] right)
        {
            return Extensions
                .Join(
                    left.AsOrderedBy(NumberInString),
                    right.AsOrderedBy(NumberInString),
                    (x, y) => $"{x} {y}",
                    r => NumberInString(r.Split(' ')[0]))
                .ToArray();
        }

        public static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, new string[] { }, new string[] { }),
                (new[] { "L0", "L1", "L2" }, new string[] { }, new string[] { }),
                (new string[] { }, new[] { "R0", "R1", "R2" }, new string[] { }),

                (new[] { "L0" }, new[] { "R0" }, new string[] { "L0 R0" }),
                (new[] { "L0", "L1", "L2" }, new[] { "R0", "R1", "R2" }, new string[] { "L0 R0", "L1 R1", "L2 R2" }),

                (new[] { "L0", "L1", "L2" }, new[] { "R2", "R3", "R4" }, new string[] { "L2 R2" }),
                (new[] { "L2", "L3", "L4" }, new[] { "R0", "R1", "R2" }, new string[] { "L2 R2" }),

                (new[] { "L0", "L0", "L1", "L2", "L2" }, new[] { "R0", "R1", "R1", "R2" }, new string[] { "L0 R0", "L0 R0", "L1 R1", "L1 R1", "L2 R2", "L2 R2" }),
                (new[] { "L0", "L0", "L1", "L3", "L3" }, new[] { "R1", "R1", "R2", "R2", "R3" }, new string[] { "L1 R1", "L1 R1", "L3 R3", "L3 R3" }),
            
                (new[] { "L-1", "L1", "L1", "L2", "L3", "L3", "L4" }, new[] { "R0", "R1", "R2", "R2", "R3", "R3" }, new string[] { "L1 R1", "L1 R1", "L2 R2", "L2 R2", "L3 R3", "L3 R3", "L3 R3", "L3 R3" })
            };

            var linqTestCases = testCases
                .Select(c => (c.Item1, c.Item2, Enumerable
                    .Join(
                        c.Item1, c.Item2,
                        NumberInString, NumberInString,
                        (x ,y) => $"{x} {y}")
                    .ToArray()));

            return testCases
                .Select((c, index) => new TestCaseData(c.Item1, c.Item2).Returns(c.Item3).SetName($"Join {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}
