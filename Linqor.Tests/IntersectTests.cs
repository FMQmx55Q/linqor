using System.Collections.Generic;

namespace Linqor.Tests
{
    public class IntersectTests : BinaryOperationTests<int, int, int>
    {
        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateCases()
        {
            const string name = "Intersect";
            return new[]
            {
                BinaryTestCase.Create(name, new int[] { }, new int[] { }, new int[] { }),
                BinaryTestCase.Create(name, new int[] { }, new int[] { 1, 2, 3, 4, 5 }, new int[] { }),
                BinaryTestCase.Create(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { }, new int[] { }),
                BinaryTestCase.Create(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 6, 7 }, new int[] { 5 }),
                BinaryTestCase.Create(name, new int[] { 1, 2, 3 }, new int[] { 3, 4, 5, 6, 7 }, new int[] { 3 }),
                BinaryTestCase.Create(name, new int[] { 1, 3, 5, 7, 9 }, new int[] { 2, 4, 6, 8 }, new int[] { }),
                BinaryTestCase.Create(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5 }),
                BinaryTestCase.Create(name, new int[] { 5, 1, 4, 2, 3 }, new int[] { 2, 6, 5, 3, 4 }, new int[] { }),
                BinaryTestCase.Create(name, new int[] { 5, 4, 3, 2, 1 }, new int[] { 6, 5, 4, 3, 2 }, new int[] { })
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateInfiniteCases()
        {
            const string name = "Intersect ∞";
            yield return BinaryTestCase.Create(name, TestCases.Generate(1, 1, 2), TestCases.Generate(5, 1, 2), new[] { 25, 27, 29, 31, 33 });
        }

        protected override IEnumerable<int> Operate(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return outer.OrderedIntersect(inner, (o, i) => o.CompareTo(i));
        }
    }
}
