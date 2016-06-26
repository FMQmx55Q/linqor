using System;
using System.Collections.Generic;

namespace Linqor.Tests
{
    public class DistinctTests : UnaryOperationTests<int, int>
    {
        protected override IEnumerable<UnaryTestCase<int, int>> GetOperateCases()
        {
            var create = UnaryTestCase.GetCreator<int>("Distinct");
            return new[]
            {
                create(new int[] { }, new int[] { }),
                create(new[] { 0 }, new[] { 0 }),
                create(new[] { 0, 1, 2 }, new[] { 0, 1, 2 }),
                create(new[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new[] { 0, 1, 2, 3, 4 })
            };
        }

        protected override IEnumerable<UnaryTestCase<int, int>> GetOperateInfiniteCases()
        {
            var create = UnaryTestCase.GetCreator<int>("Distinct ∞");
            return new[]
            {
                create(TestCases.Generate(0, 2, 1), new int[] { 10, 11, 12, 13, 14 }),
                create(TestCases.Generate(0, 3, 1), new int[] { 10, 11, 12, 13, 14 })
            };
        }

        protected override IEnumerable<int> Operate(IEnumerable<int> source, Func<int, int, bool> equal)
        {
            return source.AsOrderedBy(s => s).Distinct(equal);
        }
    }
}
