using System.Collections.Generic;
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
                create(new int[] { }, new int[] { }, new string[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { }, new string[] { }),
                create(new int[] { }, new int[] { 0, 1, 2 }, new string[] { }),

                create(new int[] { 0 }, new int[] { 0 }, new string[] { "0 0" }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { "0 0", "1 1", "2 2" }),

                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "2 2" }),
                create(new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "2 2" }),

                create(new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { "0 0", "0 0", "1 1", "1 1", "2 2", "2 2" }),
                create(new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "1 1", "1 1", "3 3", "3 3" }),
            
                create(new int[] { -1, 1, 1, 2, 3, 3, 4 }, new[] { 0, 1, 2, 2, 3, 3 }, new string[] { "1 1", "1 1", "2 2", "2 2", "3 3", "3 3", "3 3", "3 3" })
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

        protected override IEnumerable<string> Operate(IEnumerable<int> left, IEnumerable<int> right)
        {
            return left.OrderedJoin(right, l => l, r => r, (l, r) => l + " " + r, (l, r) => l.CompareTo(r));
        }
    }
}
