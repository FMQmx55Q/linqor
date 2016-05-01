using System.Collections.Generic;

namespace Linqor.Tests
{
    public class UnionTests : BinaryOperationTests<int, int, int>
    {
        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateCases()
        {
            const string name = "Union";

            return new[]
            {
                BinaryTestCase.Create(name, new int[] { }, new int[] { }, new int[] { }),
                BinaryTestCase.Create(name, new int[] { }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 }),
                BinaryTestCase.Create(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { }, new int[] { 1, 2, 3, 4, 5 }),
                BinaryTestCase.Create(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 }),
                BinaryTestCase.Create(name, new int[] { 1, 2, 3 }, new int[] { 3, 4, 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 }),
                BinaryTestCase.Create(name, new int[] { 2, 4, 6, 8 }, new int[] { 1, 3, 5, 7, 9 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }),
                BinaryTestCase.Create(name, new int[] { 1, 9, 7, 3, 5 }, new int[] { 6, 4, 2, 8 }, new int[] { 1, 6, 4, 2, 8, 9, 7, 3, 5 }),
                BinaryTestCase.Create(name, new int[] { 9, 7, 5, 3, 1 }, new int[] { 8, 6, 4, 2 }, new int[] { 8, 6, 4, 2, 9, 7, 5, 3, 1 }),
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateInfiniteCases()
        {
            const string name = "Union ∞";
            yield return BinaryTestCase.Create(name, TestCases.Generate(1, 1, 2), TestCases.Generate(2, 1, 2), new[] { 11, 12, 13, 14, 15 });
        }

        protected override IEnumerable<int> Operate(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return outer.OrderedUnion(inner, (o, i) => o.CompareTo(i));
        }
    }
}
