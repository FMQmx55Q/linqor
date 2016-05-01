using System;
using System.Collections.Generic;

namespace Linqor.Tests
{
    public static class BinaryTestCase
    {
        public static Func<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>, BinaryTestCase<T, T, T>> GetCreator<T>(string name)
        {
            return GetCreator<T, T, T>(name);
        }
        public static Func<IEnumerable<TOuter>, IEnumerable<TInner>, IEnumerable<TExpected>, BinaryTestCase<TOuter, TInner, TExpected>> GetCreator<TOuter, TInner, TExpected>(string name)
        {
            return (outer, inner, expected) => BinaryTestCase.Create<TOuter, TInner, TExpected>(name, outer, inner, expected);
        }

        public static BinaryTestCase<TOuter, TInner, TExpected> Create<TOuter, TInner, TExpected>(string name, IEnumerable<TOuter> outer, IEnumerable<TInner> inner, IEnumerable<TExpected> expected)
        {
            return new BinaryTestCase<TOuter, TInner, TExpected>
            {
                Name = name,
                Outer = outer,
                Inner = inner,
                Expected = expected
            };
        }
    }
    public class BinaryTestCase<TOuter, TInner, TExpected>
    {
        public string Name;
        public IEnumerable<TOuter> Outer;
        public IEnumerable<TInner> Inner;
        public IEnumerable<TExpected> Expected;
    }
}
