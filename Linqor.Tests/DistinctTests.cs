using System;
using System.Collections.Generic;

namespace Linqor.Tests
{
    public class DistinctTests : UnaryOperationTests
    {
        protected override IEnumerable<UnaryTestCase<int>> GetOperateCases()
        {
            const string name = "Distinct";
            return new[]
            {
                UnaryTestCase.Create(name, new int[] { }, new int[] { }),
                UnaryTestCase.Create(name, new[] { 0 }, new[] { 0 }),
                UnaryTestCase.Create(name, new[] { 0, 1, 2 }, new[] { 0, 1, 2 }),
                UnaryTestCase.Create(name, new[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new[] { 0, 1, 2, 3, 4 })
            };
        }

        protected override IEnumerable<UnaryTestCase<int>> GetOperateInfiniteCases()
        {
            const string name = "Distinct ∞";
            return new[]
            {
                UnaryTestCase.Create(name, TestCases.Generate(0, 2, 1), new int[] { 10, 11, 12, 13, 14 }),
                UnaryTestCase.Create(name, TestCases.Generate(0, 3, 1), new int[] { 10, 11, 12, 13, 14 })
            };
        }

        protected override IEnumerable<T> Operate<T>(IEnumerable<T> source)
        {
            return source.OrderedDistinct();
        }

        protected override IEnumerable<T> OperateByKey<T, TKey>(IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.OrderedDistinct(keySelector);
        }

        protected override IEnumerable<T> OperateByEquals<T>(IEnumerable<T> source, Func<T, T, bool> equal)
        {
            return source.OrderedDistinct(equal);
        }
    }
}
