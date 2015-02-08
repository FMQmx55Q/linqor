using System;
using System.Collections.Generic;

namespace Linqor.Tests
{
    public class DistinctTests : UnaryOperationTests
    {
        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>>> GetOperateCases()
        {
            const string name = "Distinct";
            yield return TestCases.CreateUnaryCase(name, new int[] { }, new int[] { });
            yield return TestCases.CreateUnaryCase(name, new[] { 0 }, new[] { 0 });
            yield return TestCases.CreateUnaryCase(name, new[] { 0, 1, 2 }, new[] { 0, 1, 2 });
            yield return TestCases.CreateUnaryCase(name, new[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new[] { 0, 1, 2, 3, 4 });
        }

        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>>> GetOperateInfiniteCases()
        {
            const string name = "Distinct ∞";
            yield return TestCases.CreateUnaryCase(name, TestCases.Generate(0, 2, 1), new int[] { 10, 11, 12, 13, 14 });
            yield return TestCases.CreateUnaryCase(name, TestCases.Generate(0, 3, 1), new int[] { 10, 11, 12, 13, 14 });
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
