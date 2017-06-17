using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    public class ExceptTests
    {
        public static IEnumerable<TestCaseData> GetOperateCases()
        {
            var testCases = new[]
            {
                (new int[] { }, new int[] { }, new string[] { }),
                (new int[] { 0, 1, 2 }, new int[] { }, new string[] { "L-0-0", "L-1-1", "L-2-2" }),
                (new int[] { }, new int[] { 0, 1, 2 }, new string[] { }),

                (new int[] { 0 }, new int[] { 0 }, new string[] { }),
                (new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { }),

                (new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "L-0-0", "L-1-1" }),
                (new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "L-1-3", "L-2-4" }),

                (new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { }),
                (new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "L-0-0" }),

                (new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, new string[] { "L-0-0", "L-1-1", "L-2-2", "L-3-3", "L-4-4" }),
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Except", testCase, func);
        }

        public static IEnumerable<TestCaseData> GetOperateInfiniteCases()
        {
            var testCases = new[]
            {
                (TestCase.Generate(0, 1, 1), TestCase.Generate(15, 1, 1), new string[] { "L-10-10", "L-11-11", "L-12-12", "L-13-13", "L-14-14" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Except ∞", testCase, func);
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>> GetFuncs()
        {
            return new Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>[]
            {
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Except(
                        right.ToEntities("R").AsOrderedBy(r => r.Value),
                        (l, r) => l.CompareTo(r))
                    .Select(e => e.Key),
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Except(
                        right.ToEntities("R").AsOrderedBy(r => r.Value))
                    .Select(e => e.Key)
            };
        }
    }
}
