using System.Collections.Generic;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class GroupJoinTests : BinaryOperationTests<int, int, string>
    {
        protected override IEnumerable<BinaryTestCase<int, int, string>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("GroupJoin");
            return new[]
            {
                create(new int[] { 1, 1, 2, 3, 3, 4 }, new[] { 0, 1, 2, 2, 3, 3 }, new[] { "1 {1}", "1 {1}", "2 {2, 2}", "3 {3, 3}", "3 {3, 3}", "4 {}" })
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, string>> GetOperateInfiniteCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("GroupJoin ∞");
            return new[]
            {
                create(TestCases.Generate(0, 2, 1), TestCases.Generate(1, 1, 1), new[] { "5 {5}", "5 {5}", "6 {6}", "6 {6}", "7 {7}" })
            };
        }

        protected override IEnumerable<string> Operate(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return outer.OrderedGroupJoin(inner, t => t, t => t, (left, right) => string.Format("{0} {{{1}}}", left, string.Join(", ", right)), (l, r) => l.CompareTo(r));
        }
    }
}
