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
                create(new int[] { 1, 2, 2, 3, 3, 3 }, new[] { "1: { 1 }", "2: { 2, 2 }", "3: { 3, 3, 3 }" })
            };
        }

        protected override IEnumerable<UnaryTestCase<int, string>> GetOperateInfiniteCases()
        {
            var create = UnaryTestCase.GetCreator<int, string>("GroupBy ∞");
            return new[] 
            {
                create(TestCases.Generate(1, 2, 1), new[] { "11: { 11, 11 }", "12: { 12, 12 }", "13: { 13, 13 }", "14: { 14, 14 }", "15: { 15, 15 }" })
            };
        }

        protected override IEnumerable<string> Operate(IEnumerable<int> source, Func<int, int, bool> equal)
        {
            return source
                .AsOrderedBy(s => s)
                .GroupBy(equal)
                .Select(grouping => string.Format("{0}: {{ {1} }}", grouping.Key, string.Join(", ", grouping)));
        }
    }
}
