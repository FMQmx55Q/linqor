using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    [TestFixture]
    public class GroupByTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public string[] GroupBy(string[] source)
        {
            return Extensions
                .GroupBy(source.AsOrderedBy(NumberInString))
                .Select(g => $"{g.Key}: [ {string.Join(", ", g)} ]")
                .ToArray();
        }

        public static IEnumerable<TestCaseData> GetTestCases()
        {
            var testCases = new[]
            {
                (new string[] { }, new string[] { }),
                (new[] { "S1", "S2", "S2", "S3", "S3", "S3" }, new[] { "1: [ S1 ]", "2: [ S2, S2 ]", "3: [ S3, S3, S3 ]" })
            };

            var linqTestCases = testCases
                .Select(c => (c.Item1, Enumerable
                    .GroupBy(c.Item1, NumberInString)
                    .Select(g => $"{g.Key}: [ {string.Join(", ", g)} ]")
                    .ToArray()));

            return testCases.Concat(linqTestCases)
                .Select((c, index) => new TestCaseData((object)c.Item1).Returns(c.Item2).SetName($"GroupBy {Helpers.Get2DID(index, testCases.Length)}"));
        }
    }
}
