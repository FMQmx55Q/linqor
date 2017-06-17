using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    public class IntersectTests
    {
        public static IEnumerable<TestCaseData> GetOperateCases()
        {
            var testCases = new[]
            {
                (new int[] { }, new int[] { }, new string[] { }),
                (new int[] { 0, 1, 2 }, new int[] { }, new string[] { }),
                (new int[] { }, new int[] { 0, 1, 2 }, new string[] { }),
                
                (new int[] { 0 }, new int[] { 0 }, new string[] { "L-0-0" }),
                (new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { "L-0-0", "L-1-1", "L-2-2" }),

                (new int[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 }, new string[] { "L-0-0", "L-1-1", "L-2-2", "L-4-3", "L-7-4" }),
                
                (new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "L-2-2" }),
                (new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "L-0-2" }),

                (new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { "L-0-0", "L-2-1", "L-3-2" }),
                (new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "L-2-1", "L-3-3" }),
            
                (new int[] { 1, 3, 5, 7, 9 }, new int[] { 2, 4, 6, 8 }, new string[] { }),
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Intersect", testCase, func);
        }

        public static IEnumerable<TestCaseData> GetOperateInfiniteCases()
        {
            var testCases = new[]
            {
                (TestCase.Generate(1, 1, 2), TestCase.Generate(5, 1, 2), new string[] { "L-12-25", "L-13-27", "L-14-29", "L-15-31", "L-16-33" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Intersect ∞", testCase, func);
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>> GetFuncs()
        {
            return new Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>[]
            {
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                        .Intersect(
                            right.ToEntities("R").AsOrderedBy(r => r.Value),
                            (l, r) => l.CompareTo(r))
                        .Select(e => e.Key),
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                        .Intersect(
                            right.ToEntities("R").AsOrderedBy(r => r.Value))
                        .Select(e => e.Key)
            };
        }
    }
}
