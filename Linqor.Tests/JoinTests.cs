using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class JoinTests
    {
        public static IEnumerable<TestCaseData> GetOperateCases()
        {
            var testCases = new[]
            {
                (new int[] { }, new int[] { }, new string[] { }),
                (new int[] { 0, 1, 2 }, new int[] { }, new string[] { }),
                (new int[] { }, new int[] { 0, 1, 2 }, new string[] { }),

                (new int[] { 0 }, new int[] { 0 }, new string[] { "L-0-0 R-0-0" }),
                (new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { "L-0-0 R-0-0", "L-1-1 R-1-1", "L-2-2 R-2-2" }),

                (new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "L-2-2 R-0-2" }),
                (new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "L-0-2 R-2-2" }),

                (new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { "L-0-0 R-0-0", "L-1-0 R-0-0", "L-2-1 R-1-1", "L-2-1 R-2-1", "L-3-2 R-3-2", "L-4-2 R-3-2" }),
                (new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "L-2-1 R-0-1", "L-2-1 R-1-1", "L-3-3 R-4-3", "L-4-3 R-4-3" }),
            
                (new int[] { -1, 1, 1, 2, 3, 3, 4 }, new[] { 0, 1, 2, 2, 3, 3 }, new string[] { "L-1-1 R-1-1", "L-2-1 R-1-1", "L-3-2 R-2-2", "L-3-2 R-3-2", "L-4-3 R-4-3", "L-4-3 R-5-3", "L-5-3 R-4-3", "L-5-3 R-5-3" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Join", testCase, func);
        }

        public static IEnumerable<TestCaseData> GetOperateInfiniteCases()
        {
            var testCases = new[]
            {
                (TestCase.Generate(0, 2, 1), TestCase.Generate(1, 1, 1), new[] { "L-12-6 R-5-6", "L-13-6 R-5-6", "L-14-7 R-6-7", "L-15-7 R-6-7", "L-16-8 R-7-8" })
            };

            return from func in GetFuncs()
                from testCase in testCases
                select TestCase.Binary("Join ∞", testCase, func);
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>> GetFuncs()
        {
            return new Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>[]
            {
                (left, right) =>  left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Join(
                        right.ToEntities("R").AsOrderedBy(r => r.Value),
                        (l, r) => l.Key + " " + r.Key,
                        (l, r) => l.CompareTo(r)),
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Join(
                        right.ToEntities("R").AsOrderedBy(r => r.Value),
                        (l, r) => l.Key + " " + r.Key)
            };
        }
    }
}
