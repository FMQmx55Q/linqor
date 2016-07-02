using System.Collections.Generic;
using System.Linq;

namespace Linqor.Tests
{
    public class IntersectTests : BinaryOperationTests<int, int, string>
    {
        protected override IEnumerable<BinaryTestCase<int, int, string>> GetOperateCases()
        {
            var create = BinaryTestCase.GetCreator<int, int, string>("Intersect");
            return new[]
            {
                create(new int[] { }, new int[] { }, new string[] { }),
                create(new int[] { 0, 1, 2 }, new int[] { }, new string[] { }),
                create(new int[] { }, new int[] { 0, 1, 2 }, new string[] { }),
                
                create(new int[] { 0 }, new int[] { 0 }, new string[] { "L-0-0" }),
                create(new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new string[] { "L-0-0", "L-1-1", "L-2-2" }),

                create(new int[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 }, new string[] { "L-0-0", "L-1-1", "L-2-2", "L-4-3", "L-7-4" }),
                
                create(new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new string[] { "L-2-2" }),
                create(new int[] { 2, 3, 4 }, new int[] { 0, 1, 2 }, new string[] { "L-0-2" }),

                create(new int[] { 0, 0, 1, 2, 2 }, new int[] { 0, 1, 1, 2 }, new string[] { "L-0-0", "L-2-1", "L-3-2" }),
                create(new int[] { 0, 0, 1, 3, 3 }, new int[] { 1, 1, 2, 2, 3 }, new string[] { "L-2-1", "L-3-3" }),
            
                create(new int[] { 1, 3, 5, 7, 9 }, new int[] { 2, 4, 6, 8 }, new string[] { }),
            };
        }

        protected override IEnumerable<BinaryTestCase<int, int, string>> GetOperateInfiniteCases()
        {
            const string name = "Intersect ∞";
            yield return BinaryTestCase.Create(name, Extensions.Generate(1, 1, 2), Extensions.Generate(5, 1, 2), new string[] { "L-12-25", "L-13-27", "L-14-29", "L-15-31", "L-16-33" });
        }

        protected override IEnumerable<string> Operate(IEnumerable<int> left, IEnumerable<int> right)
        {
            return left.ToEntities("L").AsOrderedBy(l => l.Value)
                .Intersect(
                    right.ToEntities("R").AsOrderedBy(r => r.Value),
                    (l, r) => l.CompareTo(r))
                .Select(e => e.Key);
        }
    }
}
