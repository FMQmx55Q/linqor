using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class GroupJoinTests
    {
        public static IReadOnlyList<TestCaseData> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("GroupJoin");
            var testCases = new[]
            {
                create(new int[] { }, new int[] { }, new string[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { }, new string[] { "L-0-0 {  }", "L-1-1 {  }", "L-2-2 {  }" }),
                create(new int[] { }, new int[] { 0, 1, 2 }, new string[] { }),
                
                create(new int[] { 0 }, new int[] { 0 }, new string[] { "L-0-0 { R-0-0 }" }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { "L-0-0 { R-0-0 }", "L-1-1 { R-1-1 }", "L-2-2 { R-2-2 }" }),

                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "L-0-0 {  }", "L-1-1 {  }", "L-2-2 { R-0-2 }" }),
                create(new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "L-0-2 { R-2-2 }", "L-1-3 {  }", "L-2-4 {  }" }),


                create(new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { "L-0-0 { R-0-0 }", "L-1-0 { R-0-0 }", "L-2-1 { R-1-1, R-2-1 }", "L-3-2 { R-3-2 }", "L-4-2 { R-3-2 }" }),
                create(new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "L-0-0 {  }", "L-1-0 {  }", "L-2-1 { R-0-1, R-1-1 }", "L-3-3 { R-4-3 }", "L-4-3 { R-4-3 }" }),

                create(new int[] { 0, 1, 1, 2, 3, 3, 4 }, new[] { -1, 1, 2, 2, 3, 3 }, new string[] { "L-0-0 {  }", "L-1-1 { R-1-1 }", "L-2-1 { R-1-1 }", "L-3-2 { R-2-2, R-3-2 }", "L-4-3 { R-4-3, R-5-3 }", "L-5-3 { R-4-3, R-5-3 }", "L-6-4 {  }" })
            };

            return GetOperations().SelectMany(testCases.ToTestCases).ToArray();
        }

        public static IReadOnlyList<TestCaseData> GetOperateInfiniteCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("GroupJoin ∞");
            var testCases = new[]
            {
                create(Extensions.Generate(0, 2, 1), Extensions.Generate(1, 2, 1), new[] { "L-10-5 { R-8-5, R-9-5 }", "L-11-5 { R-8-5, R-9-5 }", "L-12-6 { R-10-6, R-11-6 }", "L-13-6 { R-10-6, R-11-6 }", "L-14-7 { R-12-7, R-13-7 }" })
            };

            return GetOperations().SelectMany(testCases.ToTestCases).ToArray();
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>> GetOperations()
        {
            return new Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>[]
            {
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .GroupJoin(
                        right.ToEntities("R").AsOrderedBy(r => r.Value),
                        (l, r) => string.Format("{0} {{ {1} }}", l.Key, string.Join(", ", r.Select(e => e.Key))),
                        (l, r) => l.CompareTo(r)),
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .GroupJoin(
                        right.ToEntities("R").AsOrderedBy(r => r.Value),
                        (l, r) => string.Format("{0} {{ {1} }}", l.Key, string.Join(", ", r.Select(e => e.Key))))
            };
        }
    }
}
