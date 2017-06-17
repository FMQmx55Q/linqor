using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    public static class DistinctTests
    {
        public static IEnumerable<TestCaseData> GetOperateCases()
        {
            var testCases = new[]
            {
                (new int[] { }, new string[] { }),
                (new int[] { 0 }, new string[] { "S-0-0" }),
                (new int[] { 0, 1, 2 }, new string[] { "S-0-0", "S-1-1", "S-2-2" }),
                (new int[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new string[] { "S-0-0", "S-1-1", "S-2-2", "S-4-3", "S-7-4" })
            };
            
            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Unary("Distinct", testCase, func);
        }

        public static IEnumerable<TestCaseData> GetOperateInfiniteCases()
        {
            var testCases = new[]
            {
                (TestCase.Generate(0, 2, 1), new string[] { "S-20-10", "S-22-11", "S-24-12", "S-26-13", "S-28-14" }),
                (TestCase.Generate(0, 3, 1), new string[] { "S-30-10", "S-33-11", "S-36-12", "S-39-13", "S-42-14" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Unary("Distinct ∞", testCase, func);
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<string>>> GetFuncs()
        {
            return new Func<IEnumerable<int>, IEnumerable<string>>[]
            {
                (source) => source.ToEntities("S").AsOrderedBy(s => s.Value)
                    .Distinct((l, r) => l.Equals(r))
                    .Select(e => e.Key),
                (source) => source.ToEntities("S").AsOrderedBy(s => s.Value)
                    .Distinct()
                    .Select(e => e.Key)
            };
        }
    }
}
