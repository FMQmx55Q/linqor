using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    public static class TestCase
    {
        public static TestCaseData Binary<TLeft, TRight, TExpected>(
            string name,
            (IEnumerable<TLeft> Left, IEnumerable<TRight> Right, IEnumerable<TExpected> Expected) testCase,
            Func<IEnumerable<TLeft>, IEnumerable<TRight>, IEnumerable<TExpected>> func)
        {
            string details = string.Format(" {{ {0} }} {{ {1} }}", string.Join(", ", testCase.Left.Take(10)), string.Join(", ", testCase.Right.Take(10)));

            return new TestCaseData(testCase.Left, testCase.Right, func).Returns(testCase.Expected).SetName(name);
        }

        public static TestCaseData Unary<TSource, TExpected>(
            string name,
            (IEnumerable<TSource> Source, IEnumerable<TExpected> Expected) testCase,
            Func<IEnumerable<TSource>, IEnumerable<TExpected>> func)
        {
            string details = string.Format(" {{ {0} }}", string.Join(", ", testCase.Source.Take(10)));

            return new TestCaseData(testCase.Source, func).Returns(testCase.Expected).SetName(name + details);
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