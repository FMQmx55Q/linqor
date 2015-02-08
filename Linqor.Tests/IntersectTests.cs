using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Linqor.Tests
{
    public class IntersectTests : BinaryOperationTests
    {
        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>> GetOperateCases()
        {
            const string name = "Intersect";
            yield return TestCases.CreateBinaryCase(name, new int[] { }, new int[] { }, new int[] { });
            yield return TestCases.CreateBinaryCase(name, new int[] { }, new int[] { 1, 2, 3, 4, 5 }, new int[] { });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { }, new int[] { });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { 5, 6, 7 }, new int[] { 5 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 2, 3 }, new int[] { 3, 4, 5, 6, 7 }, new int[] { 3 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 3, 5, 7, 9 }, new int[] { 2, 4, 6, 8 }, new int[] { });
            yield return TestCases.CreateBinaryCase(name, new int[] { 1, 2, 3, 4, 5 }, new int[] { 2, 3, 4, 5, 6 }, new int[] { 2, 3, 4, 5 });
            yield return TestCases.CreateBinaryCase(name, new int[] { 5, 1, 4, 2, 3 }, new int[] { 2, 6, 5, 3, 4 }, new int[] { });
            yield return TestCases.CreateBinaryCase(name, new int[] { 5, 4, 3, 2, 1 }, new int[] { 6, 5, 4, 3, 2 }, new int[] { });
        }

        protected override IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>> GetOperateInfiniteCases()
        {
            const string name = "Intersect  ∞";
            yield return TestCases.CreateBinaryCase(name, TestCases.Generate(1, 1, 2), TestCases.Generate(5, 1, 2), new[] { 25, 27, 29, 31, 33 });
        }

        protected override IEnumerable<T> Operate<T>(IEnumerable<T> outer, IEnumerable<T> inner)
        {
            return outer.OrderedIntersect(inner);
        }

        protected override IEnumerable<T> OperateByKey<T, TKey>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, TKey> keySelector)
        {
            return outer.OrderedIntersect(inner, keySelector);
        }

        protected override IEnumerable<T> OperateByCompare<T>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            return outer.OrderedIntersect(inner, compare);
        }

        [Test]
        public void Should_OrderedIntersect_Descending_Enumerables_With_Custom_Comparer()
        {
            // Arrange
            var first = new[] { 5, 4, 3, 2, 1 };
            var second = new[] { 6, 5, 4, 3, 2 };

            // Act
            int[] result = second.OrderedIntersect(first, (x, y) => x.CompareTo(y) * -1).ToArray();

            // Assert
            Assert.That(result, Is.EqualTo(new[] { 5, 4, 3, 2 }));
        }
    }
}
