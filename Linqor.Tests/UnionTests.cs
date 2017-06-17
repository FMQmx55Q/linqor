using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    public class UnionTests
    {
        public static IEnumerable<TestCaseData> GetOperateCases()
        {
            var testCases = new[]
            {
                (new int[] { }, new int[] { }, new string[] { }),
                (new int[] { 0, 1, 2 }, new int[] { }, new string[] { "L-0-0", "L-1-1", "L-2-2" }),
                (new int[] { }, new int[] { 0, 1, 2 }, new string[] { "R-0-0", "R-1-1", "R-2-2" }),

                (new int[] { 0 }, new int[] { 0 }, new string[] { "L-0-0" }),
                (new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { "L-0-0", "L-1-1", "L-2-2" }),

                (new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "L-0-0", "L-1-1", "L-2-2", "R-1-3", "R-2-4" }),
                (new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "R-0-0", "R-1-1", "L-0-2", "L-1-3", "L-2-4" }),

                (new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { "L-0-0", "L-2-1", "L-3-2" }),
                (new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "L-0-0", "L-2-1", "R-2-2", "L-3-3" }),
            
                (new int[] { 2, 4, 6, 8 }, new int[] { 1, 3, 5, 7, 9 }, new string[] { "R-0-1", "L-0-2", "R-1-3", "L-1-4", "R-2-5", "L-2-6", "R-3-7", "L-3-8", "R-4-9" }),
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Union", testCase, func);
        }

        public static IEnumerable<TestCaseData> GetOperateInfiniteCases()
        {
            var testCases = new[]
            {
                (TestCase.Generate(1, 1, 2), TestCase.Generate(2, 1, 2), new[] { "L-5-11", "R-5-12", "L-6-13", "R-6-14", "L-7-15" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Union ∞", testCase, func);
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>> GetFuncs()
        {
            return new Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>[]
            {
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Union(
                        right.ToEntities("R").AsOrderedBy(r => r.Value),
                        (l, r) => l.CompareTo(r))
                    .Select(e => e.Key),
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Union(
                        right.ToEntities("R").AsOrderedBy(r => r.Value))
                    .Select(e => e.Key)
            };
        }
    }
}
