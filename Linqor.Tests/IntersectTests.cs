using System.Collections.Generic;

namespace Linqor.Tests
{
    public class IntersectTests : BinaryOperationTests<int, int, int>
    {
        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int>("Intersect");
            return new[]
            {
                create(new int[] { }, new int[] { }, new int[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { }, new int[] { }),
                create(new int[] { }, new int[] { 0, 1, 2 }, new int[] { }),
                
                create(new int[] { 0 }, new int[] { 0 }, new int[] { 0 }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }),

                create(new int[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 }, new int[] { 0, 1, 2, 3, 4 }),
                
                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new int[] { 2 }),
                create(new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new int[] { 2 }),

                create(new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new int[] { 0, 1, 2 }),
                create(new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new int[] { 1, 3 }),
            
                create(new int[] { 1, 3, 5, 7, 9 }, new int[] { 2, 4, 6, 8 }, new int[] { }),
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateInfiniteCases()
        {
            const string name = "Intersect ∞";
            yield return BinaryTestCase.Create(name, TestCases.Generate(1, 1, 2), TestCases.Generate(5, 1, 2), new[] { 25, 27, 29, 31, 33 });
        }

        protected override IEnumerable<int> Operate(IEnumerable<int> left, IEnumerable<int> right)
        {
            return left
                .AsOrderedBy(l => l)
                .Intersect(
                    right.AsOrderedBy(r => r),
                    (l, r) => l.CompareTo(r));
        }
    }
}
