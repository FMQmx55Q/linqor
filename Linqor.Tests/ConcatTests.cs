using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor.Tests
{
    public class ConcatTests : BinaryOperationTests<int, int, string>
    {
        protected override IReadOnlyList<BinaryTestCase<int, int, string>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("Concat");
            return new[]
            {
                create(new int[] { }, new int[] { }, new string[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { }, new string[] { "L-0-0", "L-1-1", "L-2-2" }),
                create(new int[] { }, new int[] { 0, 1, 2 }, new string[] { "R-0-0", "R-1-1", "R-2-2" }),

                create(new int[] { 0 }, new int[] { 0 }, new string[] { "L-0-0", "R-0-0" }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { "L-0-0", "R-0-0", "L-1-1", "R-1-1", "L-2-2", "R-2-2" }),

                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "L-0-0", "L-1-1", "L-2-2", "R-0-2", "R-1-3", "R-2-4" }),
                create(new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "R-0-0", "R-1-1", "L-0-2", "R-2-2", "L-1-3", "L-2-4" }),

                create(new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { "L-0-0", "L-1-0", "R-0-0", "L-2-1", "R-1-1", "R-2-1", "L-3-2", "L-4-2", "R-3-2" }),
                create(new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "L-0-0", "L-1-0", "L-2-1", "R-0-1", "R-1-1", "R-2-2", "R-3-2", "L-3-3", "L-4-3", "R-4-3" })
            };
        }

        protected override IReadOnlyList<BinaryTestCase<int, int, string>> GetOperateInfiniteCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("Concat ∞");
            return new[]
            {
                create(Extensions.Generate(0, 1, 1), Extensions.Generate(1, 1, 1), new string[] { "R-4-5", "L-6-6", "R-5-6", "L-7-7", "R-6-7" })
            };
        }

        protected override IReadOnlyList<Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>> GetOperations()
        {
            return new Func<IEnumerable<int>, IEnumerable<int>, IEnumerable<string>>[]
            {
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Concat(
                        right.ToEntities("R").AsOrderedBy(r => r.Value),
                        (l, r) => l.CompareTo(r))
                    .Select(e => e.Key),
                (left, right) => left.ToEntities("L").AsOrderedBy(l => l.Value)
                    .Concat(
                        right.ToEntities("R").AsOrderedBy(r => r.Value))
                    .Select(e => e.Key)
            };
        }
    }
}
