using System.Collections.Generic;

namespace Linqor.Tests
{
    public class ConcatTests : BinaryOperationTests<int, int, int>
    {
        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int>("Concat");
            return new[]
            {
                create(new int[] { }, new int[] { }, new int[] { }),
                create(new int[] { 0 }, new int[] { 0 }, new int[] { 0, 0 }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 0, 1, 1, 2, 2 }),
                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new int[] { 0, 1, 2, 2, 3, 4 })
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, int>> GetOperateInfiniteCases()
        {
            var create = BinaryTestCase.GetCreator<int>("Concat ∞");
            return new[]
            {
                create(TestCases.Generate(0, 1, 1), TestCases.Generate(1, 1, 1), new[] { 5, 6, 6, 7, 7 })
            };
        }

        protected override IEnumerable<int> Operate(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return outer.OrderedConcat(inner, (o, i) => o.CompareTo(i));
        }
    }
}
