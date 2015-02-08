using System;
using System.Collections.Generic;

namespace Linqor.Tests
{
    public class ConcatTests : BinaryOperationTests
    {
        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>> GetOperateCases()
        {
            const string name = "Concat";
            yield return TestCases.CreateBinaryCase(name, new int[] { }, new int[] { }, new int[] { });
            yield return TestCases.CreateBinaryCase(name, new int[] { 0 }, new int[] { 0 }, new int[] { 0, 0 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 0, 1, 2 }, new int[] { 0, 1, 2 }, new int[] { 0, 0, 1, 1, 2, 2 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 0, 1, 2 }, new int[] { 2, 3, 4 }, new int[] { 0, 1, 2, 2, 3, 4 });
        }

        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>> GetOperateInfiniteCases()
        {
            const string name = "Concat ∞";
            yield return TestCases.CreateBinaryCase(name, TestCases.Generate(0, 1, 1), TestCases.Generate(1, 1, 1), new[] { 5, 6, 6, 7, 7 });
        }

        protected override IEnumerable<T> Operate<T>(IEnumerable<T> outer, IEnumerable<T> inner)
        {
            return outer.OrderedConcat(inner);
        }

        protected override IEnumerable<T> OperateByKey<T, TKey>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, TKey> keySelector)
        {
            return outer.OrderedConcat(inner, keySelector);
        }

        protected override IEnumerable<T> OperateByCompare<T>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            return outer.OrderedConcat(inner, compare);
        }
    }
}
