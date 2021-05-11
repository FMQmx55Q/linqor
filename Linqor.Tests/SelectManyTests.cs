using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    [TestFixture]
    public class SelectManyTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] SelectMany(string[] source, string[][] result)
        {
            return Extensions
                .SelectMany(
                    source.AsOrderedBy(NumberInString),
                    new string[] { }.AsOrderedBy(NumberInString),
                    (item, index) => result[index].AsOrderedBy(NumberInString))
                .ToArray();
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, Repeat(new string[] { }, 1), new string[] { }),
                (new [] { "L0", "L1", "L2" }, Repeat(new string[] { }, 3), new string[] { }),
                (new string[] { }, Repeat(new [] { "R0", "R1", "R2" }, 1), new string[] { }),

                (new [] { "L0", "L1" }, new[] { new[] { "R0", "R2" }, new[] { "R1", "R3" } }, new[] { "R0", "R1", "R2", "R3" })
            };

            var linqTestCases = testCases
                .Select(c => (c.Item1, c.Item2, Enumerable
                    .SelectMany(c.Item1, (item, index) => c.Item2[index])
                    .OrderBy(NumberInString)
                    .ToArray()));

            return testCases.Concat(linqTestCases)
                .Select((c, index) => new TestCaseData(c.Item1, c.Item2)
                    .Returns(c.Item3)
                    .SetName($"SelectMany {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}