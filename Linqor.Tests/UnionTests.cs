using System.Collections.Generic;

namespace Linqor.Tests
{
    public class UnionTests : BinaryOperationTests<int, int, int>
    {
        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int>("Union");
            return new[]
            {
                create(new int[] { }, new int[] { }, new int[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { }, new int[] { 0, 1, 2 }),
                create(new int[] { }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }),

                create(new int[] { 0 }, new int[] { 0 }, new int[] { 0 }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }),

                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new int[] { 0, 1, 2, 3, 4 }),
                create(new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2, 3, 4 }),

                create(new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new int[] { 0, 1, 2 }),
                create(new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new int[] { 0, 1, 2, 3 }),
            
                create(new int[] { 2, 4, 6, 8 }, new int[] { 1, 3, 5, 7, 9 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }),
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateInfiniteCases()
        {
            const string name = "Union ∞";
            yield return BinaryTestCase.Create(name, TestCases.Generate(1, 1, 2), TestCases.Generate(2, 1, 2), new[] { 11, 12, 13, 14, 15 });
        }

        protected override IEnumerable<int> Operate(IEnumerable<int> left, IEnumerable<int> right)
        {
            return left
                .AsOrderedBy(l => l)
                .Union(
                    right.AsOrderedBy(r => r),
                    (l, r) => l.CompareTo(r));
        }
    }
}
