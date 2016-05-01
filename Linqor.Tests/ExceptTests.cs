using System.Collections.Generic;

namespace Linqor.Tests
{
    public class ExceptTests : BinaryOperationTests<int, int, int>
    {
        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int>("Except");
            return new[]
            {
                create(new int[] { }, new int[] { }, new int[] { }),
                create(new int[] { 0 }, new int[] { 0 }, new int[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new int[] { 0, 1 }),
                create(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, new int[] { 0, 1, 2, 3, 4 }),
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateInfiniteCases()
        {
            var create = BinaryTestCase.GetCreator<int>("Except ∞");
            return new[]
            {
                create(TestCases.Generate(0, 1, 1), TestCases.Generate(15, 1, 1), new[] { 10, 11, 12, 13, 14 })
            };
        }

        protected override IEnumerable<int> Operate(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return outer.OrderedExcept(inner, (o, i) => o.CompareTo(i));
        }
    }
}
