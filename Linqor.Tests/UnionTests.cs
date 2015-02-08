using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    public class UnionTests : BinaryOperationTests
    {
        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>> GetOperateCases()
        {
            const string name = "Union";
            yield return TestCases.CreateBinaryCase(name, new int[] { }, new int[] { }, new int[] { });
            yield return TestCases.CreateBinaryCase(name, new int[] { }, new int[] { 1, 2, 3, 4, 5 }, new int[] { 1, 2, 3, 4, 5 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { }, new int[] { 1, 2, 3, 4, 5 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 2, 3 }, new int[] { 3, 4, 5, 6, 7 }, new int[] { 1, 2, 3, 4, 5, 6, 7 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 2, 4, 6, 8 }, new int[] { 1, 3, 5, 7, 9 }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 9, 7, 3, 5 }, new int[] { 6, 4, 2, 8 }, new int[] { 1, 6, 4, 2, 8, 9, 7, 3, 5 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 9, 7, 5, 3, 1 }, new int[] { 8, 6, 4, 2 }, new int[] { 8, 6, 4, 2, 9, 7, 5, 3, 1 });
        }

        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>> GetOperateInfiniteCases()
        {
            const string name = "Union ∞";
            yield return TestCases.CreateBinaryCase(name, TestCases.Generate(1, 1, 2), TestCases.Generate(2, 1, 2), new[] { 11, 12, 13, 14, 15 });
        }

        protected override IEnumerable<T> Operate<T>(IEnumerable<T> outer, IEnumerable<T> inner)
        {
            return outer.OrderedUnion(inner);
        }

        protected override IEnumerable<T> OperateByKey<T, TKey>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, TKey> keySelector)
        {
            return outer.OrderedUnion(inner, keySelector);
        }

        protected override IEnumerable<T> OperateByCompare<T>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            return outer.OrderedUnion(inner, compare);
        }

        [Test]
        public void Should_OrderedUnion_Descending_Enumerables_With_Custom_Comparer()
        {
            // Arrange
            var first = new[] { 9, 7, 5, 3, 1 };
            var second = new[] { 8, 6, 4, 2 };

            // Act
            int[] result = second.OrderedUnion(first, (x, y) => x.CompareTo(y) * -1).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 9, 8, 7, 6, 5, 4, 3, 2, 1 }));
        }
    }
}
