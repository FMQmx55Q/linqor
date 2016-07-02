using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class GroupByTests : UnaryOperationTests<int, string>
    {
        protected override IEnumerable<UnaryTestCase<int, string>> GetOperateCases()
        {
            var create = UnaryTestCase.GetCreator<int, string>("GroupBy");
            return new[]
            {
                create(new int[] { }, new string[] { }),
                create(new int[] { 1, 2, 2, 3, 3, 3 }, new[] { "1: { S-0-1 }", "2: { S-1-2, S-2-2 }", "3: { S-3-3, S-4-3, S-5-3 }" })
            };
        }

        protected override IEnumerable<UnaryTestCase<int, string>> GetOperateInfiniteCases()
        {
            var create = UnaryTestCase.GetCreator<int, string>("GroupBy ∞");
            return new[] 
            {
                create(Extensions.Generate(1, 2, 1), new[] { "11: { S-20-11, S-21-11 }", "12: { S-22-12, S-23-12 }", "13: { S-24-13, S-25-13 }", "14: { S-26-14, S-27-14 }", "15: { S-28-15, S-29-15 }" })
            };
        }

        protected override IEnumerable<string> Operate(IEnumerable<int> source, Func<int, int, bool> equal)
        {
            return source.ToEntities("S").AsOrderedBy(s => s.Value)
                .GroupBy(equal)
                .Select(grouping => string.Format("{0}: {{ {1} }}", grouping.Key, string.Join(", ", grouping.Select(g => g.Key))));
        }
    }
}
