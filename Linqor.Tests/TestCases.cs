using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace Linqor.Tests
{
    public static class TestCases
    {
        public static IEnumerable<UnaryTestCase<TestEntity>> ToEntityCases(this IEnumerable<UnaryTestCase<int>> testCases)
        {
            return testCases.Select(testCase => UnaryTestCase.Create(testCase.Name, testCase.Source.Select(TestEntity.Create), testCase.Expected.Select(TestEntity.Create)));
        }

        public static IEnumerable<ITestCaseData> ToTestCases<T>(this IEnumerable<UnaryTestCase<T>> testCases)
        {
            return testCases.Select(testCase => CreateUnary(testCase.Name, testCase.Source, testCase.Expected));
        }

        public static IEnumerable<BinaryTestCase<TestEntity>> ToEntityCases(this IEnumerable<BinaryTestCase<int>> testCases)
        {
            return testCases.Select(testCase => BinaryTestCase.Create(testCase.Name, testCase.Outer.Select(TestEntity.Create), testCase.Inner.Select(TestEntity.Create), testCase.Expected.Select(TestEntity.Create)));
        }

        public static IEnumerable<ITestCaseData> ToTestCases<T>(this IEnumerable<BinaryTestCase<T>> testCases)
        {
            return testCases.Select(testCase => CreateBinary(testCase.Name, testCase.Outer, testCase.Inner, testCase.Expected));
        }

        public static IEnumerable<int> Generate(int start, int count, int step)
        {
            while (true)
            {
                foreach (int value in Enumerable.Repeat(start, count))
                {
                    yield return value;
                }
                start += step;
            }
        }

        private static ITestCaseData CreateUnary<T>(string name, IEnumerable<T> source, IEnumerable<T> expected)
        {
            string details = string.Format(" {{ {0} }}", string.Join(", ", source.Take(10)));

            return new TestCaseData(source).Returns(expected).SetName(name + details);
        }

        private static ITestCaseData CreateBinary<T>(string name, IEnumerable<T> outer, IEnumerable<T> inner, IEnumerable<T> expected)
        {
            string details = string.Format(" {{ {0} }} {{ {1} }}", string.Join(", ", outer.Take(10)), string.Join(", ", inner.Take(10)));

            return new TestCaseData(outer, inner).Returns(expected).SetName(name + details);
        }
    }
}
