using System;
using System.Collections.Generic;

namespace Linqor.Tests
{
    public static class UnaryTestCase
    {
        public static Func<IEnumerable<T>, IEnumerable<T>, UnaryTestCase<T, T>> GetCreator<T>(string name)
        {
            return GetCreator<T, T>(name);
        }

        public static Func<IEnumerable<TSource>, IEnumerable<TExpected>, UnaryTestCase<TSource, TExpected>> GetCreator<TSource, TExpected>(string name)
        {
            return (source, expected) => UnaryTestCase.Create<TSource, TExpected>(name, source, expected);
        }

        public static UnaryTestCase<TSource, TExpected> Create<TSource, TExpected>(string name, IEnumerable<TSource> source, IEnumerable<TExpected> expected)
        {
            return new UnaryTestCase<TSource, TExpected>
            {
                Name = name,
                Source = source,
                Expected = expected
            };
        }
    }

    public class UnaryTestCase<TSource, TExpected>
    {
        public string Name;
        public IEnumerable<TSource> Source;
        public IEnumerable<TExpected> Expected;
    }
}
