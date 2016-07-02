using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace Linqor.Tests
{
    public static class Extensions
    {
        public static IEnumerable<ITestCaseData> ToTestCases<TSource, TExpected>(this IEnumerable<UnaryTestCase<TSource, TExpected>> testCases)
        {
            return testCases.Select(testCase => CreateUnary(testCase.Name, testCase.Source, testCase.Expected));
        }
        
        public static IEnumerable<ITestCaseData> ToTestCases<TLeft, TRight, TExpected>(this IEnumerable<BinaryTestCase<TLeft, TRight, TExpected>> testCases)
        {
            return testCases.Select(testCase => CreateBinary(testCase.Name, testCase.Left, testCase.Right, testCase.Expected));
        }

        public static IEnumerable<int> Generate(int start, int repeat, int step)
        {
            while (true)
            {
                foreach (int value in Enumerable.Repeat(start, repeat))
                {
                    yield return value;
                }
                start += step;
            }
        }

        private static ITestCaseData CreateUnary<TSource, TExpected>(string name, IEnumerable<TSource> source, IEnumerable<TExpected> expected)
        {
            string details = string.Format(" {{ {0} }}", string.Join(", ", source.Take(10)));

            return new TestCaseData(source).Returns(expected).SetName(name + details);
        }

        private static ITestCaseData CreateBinary<TLeft, TRight, TExpected>(string name, IEnumerable<TLeft> left, IEnumerable<TRight> right, IEnumerable<TExpected> expected)
        {
            string details = string.Format(" {{ {0} }} {{ {1} }}", string.Join(", ", left.Take(10)), string.Join(", ", right.Take(10)));

            return new TestCaseData(left, right).Returns(expected).SetName(name + details);
        }

        public static IEnumerable<Entity<T>> ToEntities<T>(this IEnumerable<T> source, string type)
        {
            int id = 0;
            return source
                .Select(s => new Entity<T>
                {
                    Type = type,
                    Id = id++,
                    Value = s
                });
        }
    }

    public class Entity<T>
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public T Value { get; set; }

        public string Key
        { 
            get { return Type + "-" + Id + "-" + Value; }
        }
    }
}
