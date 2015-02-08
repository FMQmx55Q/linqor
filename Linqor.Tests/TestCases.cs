using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace Linqor.Tests
{
    public static class TestCases
    {
        public static ITestCaseData CreateUnary<T>(string name, IEnumerable<T> source, IEnumerable<T> expected)
        {
            string details = string.Format(" {{ {0} }}", string.Join(", ", source.Take(10)));

            return new TestCaseData(source).Returns(expected).SetName(name + details);
        }

        public static ITestCaseData CreateBinary<T>(string name, IEnumerable<T> outer, IEnumerable<T> inner, IEnumerable<T> expected)
        {
            string details = string.Format(" {{ {0} }} {{ {1} }}", string.Join(", ", outer.Take(10)), string.Join(", ", inner.Take(10)));

            return new TestCaseData(outer, inner).Returns(expected).SetName(name + details);
        }

        public static Tuple<string, IEnumerable<int>, IEnumerable<int>> CreateUnaryCase(string name, IEnumerable<int> source, IEnumerable<int> expected)
        {
            return Tuple.Create(name, source, expected);
        }

        public static IEnumerable<Tuple<string, IEnumerable<TestEntity>, IEnumerable<TestEntity>>> ToEntityCases(this IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>>> testCases)
        {
            return testCases.Select(testCase => Tuple.Create(testCase.Item1, testCase.Item2.Select(TestEntity.Create), testCase.Item3.Select(TestEntity.Create)));
        }

        public static IEnumerable<ITestCaseData> ToTestCases<T>(this IEnumerable<Tuple<string, IEnumerable<T>, IEnumerable<T>>> testCases)
        {
            return testCases.Select(testCase => TestCases.CreateUnary(testCase.Item1, testCase.Item2, testCase.Item3));
        }

        public static Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>> CreateBinaryCase(string name, IEnumerable<int> outer, IEnumerable<int> inner, IEnumerable<int> expected)
        {
            return Tuple.Create(name, outer, inner, expected);
        }

        public static IEnumerable<Tuple<string, IEnumerable<TestEntity>, IEnumerable<TestEntity>, IEnumerable<TestEntity>>> ToEntityCases(this IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>> testCases)
        {
            return testCases.Select(testCase => Tuple.Create(testCase.Item1, testCase.Item2.Select(TestEntity.Create), testCase.Item3.Select(TestEntity.Create), testCase.Item4.Select(TestEntity.Create)));
        }

        public static IEnumerable<ITestCaseData> ToTestCases<T>(this IEnumerable<Tuple<string, IEnumerable<T>, IEnumerable<T>, IEnumerable<T>>> testCases)
        {
            return testCases.Select(testCase => TestCases.CreateBinary(testCase.Item1, testCase.Item2, testCase.Item3, testCase.Item4));
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
    }
}
