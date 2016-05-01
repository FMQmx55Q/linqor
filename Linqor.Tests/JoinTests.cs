using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class JoinTests : BinaryOperationTests<int, int, string>
    {
        protected override IEnumerable<BinaryTestCase<int, int, string>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("Join");
            return new[]
            {
                create(new int[] { 1, 1, 2, 3, 3, 4 }, new[] { 0, 1, 2, 2, 3, 3 }, new[] { "1 1", "1 1", "2 2", "2 2", "3 3", "3 3", "3 3", "3 3" })
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, string>> GetOperateInfiniteCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("Join ∞");
            return new[]
            {
                create(TestCases.Generate(0, 2, 1), TestCases.Generate(1, 1, 1), new[] { "6 6", "6 6", "7 7", "7 7", "8 8" })
            };
        }

        protected override IEnumerable<string> Operate(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return outer.OrderedJoin(inner, t => t, t => t, (left, right) => left + " " + right, (l, r) => l.CompareTo(r));
        }
    }
}
