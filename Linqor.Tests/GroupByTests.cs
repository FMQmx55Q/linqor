using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public static class GroupByTests
    {
        public static IEnumerable<TestCaseData> GetOperateCases()
        {
            var testCases = new[]
            {
                (new int[] { }, new string[] { }),
                (new int[] { 1, 2, 2, 3, 3, 3 }, new[] { "1: { S-0-1 }", "2: { S-1-2, S-2-2 }", "3: { S-3-3, S-4-3, S-5-3 }" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Unary("GroupBy", testCase, func);
        }

        public static IEnumerable<TestCaseData> GetOperateInfiniteCases()
        {
            var testCases = new[] 
            {
                (TestCase.Generate(1, 2, 1), new[] { "11: { S-20-11, S-21-11 }", "12: { S-22-12, S-23-12 }", "13: { S-24-13, S-25-13 }", "14: { S-26-14, S-27-14 }", "15: { S-28-15, S-29-15 }" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Unary("GroupBy ∞", testCase, func);
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<string>>> GetFuncs()
        {
            return new Func<IEnumerable<int>, IEnumerable<string>>[]
            {
                (source) => source.ToEntities("S").AsOrderedBy(s => s.Value)
                    .GroupBy((l, r) => l.Equals(r))
                    .Select(grouping => string.Format("{0}: {{ {1} }}", grouping.Key, string.Join(", ", grouping.Select(g => g.Key)))),
                (source) => source.ToEntities("S").AsOrderedBy(s => s.Value)
                    .GroupBy()
                    .Select(grouping => string.Format("{0}: {{ {1} }}", grouping.Key, string.Join(", ", grouping.Select(g => g.Key))))
            };
        }
    }
}
