using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class GroupJoinTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] GroupJoin(string[] left, string[] right)
        {
            return Extensions
                .GroupJoin(
                    left.AsOrderedBy(Helpers.ID),
                    right.AsOrderedBy(Helpers.ID),
                    (l, r) => $"{l}: [ {string.Join(", ", r)} ]",
                    (l, r) => l.CompareTo(r))
                .ToArray();
        }

        public static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, new string[] { }, new string[] { }),
                (new[] { "L0", "L1", "L2" }, new string[] { }, new[] { "L0: [  ]", "L1: [  ]", "L2: [  ]" }),
                (new string[] { }, new[] { "R0", "R1", "R2" }, new string[] { }),
                
                (new[] { "L0" }, new[] { "R0" }, new[] { "L0: [ R0 ]" }),
                (new[] { "L0", "L1", "L2" }, new[] { "R0", "R1", "R2" }, new[] { "L0: [ R0 ]", "L1: [ R1 ]", "L2: [ R2 ]" }),

                (new[] { "L0", "L1", "L2" }, new[] { "R2", "R3", "R4" }, new[] { "L0: [  ]", "L1: [  ]", "L2: [ R2 ]" }),
                (new[] { "L2", "L3", "L4" }, new[] { "R0", "R1", "R2" }, new[] { "L2: [ R2 ]", "L3: [  ]", "L4: [  ]" }),

                (new[] { "L0", "L0", "L1", "L2", "L2" }, new[] { "R0", "R1", "R1", "R2" }, new[] { "L0: [ R0 ]", "L0: [ R0 ]", "L1: [ R1, R1 ]", "L2: [ R2 ]", "L2: [ R2 ]" }),
                (new[] { "L0", "L0", "L1", "L3", "L3" }, new[] { "R1", "R1", "R2", "R2", "R3" }, new[] { "L0: [  ]", "L0: [  ]", "L1: [ R1, R1 ]", "L3: [ R3 ]", "L3: [ R3 ]" }),

                (new[] { "L0", "L1", "L1", "L2", "L3", "L3", "L4" }, new[] { "R-1", "R1", "R2", "R2", "R3", "R3" }, new[] { "L0: [  ]", "L1: [ R1 ]", "L1: [ R1 ]", "L2: [ R2, R2 ]", "L3: [ R3, R3 ]", "L3: [ R3, R3 ]", "L4: [  ]" })
            };

            var linqTestCases = testCases
                .Select(c => (c.Item1, c.Item2, Enumerable.GroupJoin(
                    c.Item1, c.Item2,
                    Helpers.ID, Helpers.ID,
                    (l, r) => $"{l}: [ {string.Join(", ", r)} ]").ToArray()));

            return testCases
                .Select((c, index) => new TestCaseData(c.Item1, c.Item2).Returns(c.Item3).SetName($"GroupJoin {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}
