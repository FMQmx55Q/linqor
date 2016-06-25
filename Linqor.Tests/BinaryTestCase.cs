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
        public static Func<IEnumerable<TLeft>, IEnumerable<TRight>, IEnumerable<TExpected>, BinaryTestCase<TLeft, TRight, TExpected>> GetCreator<TLeft, TRight, TExpected>(string name)
        {
            return (left, right, expected) => BinaryTestCase.Create<TLeft, TRight, TExpected>(name, left, right, expected);
        }

        public static BinaryTestCase<TLeft, TRight, TExpected> Create<TLeft, TRight, TExpected>(string name, IEnumerable<TLeft> left, IEnumerable<TRight> right, IEnumerable<TExpected> expected)
        {
            return new BinaryTestCase<TLeft, TRight, TExpected>
            {
                Name = name,
                Left = left,
                Right = right,
                Expected = expected
            };
        }
    }
    public class BinaryTestCase<TLeft, TRight, TExpected>
    {
        public string Name;
        public IEnumerable<TLeft> Left;
        public IEnumerable<TRight> Right;
        public IEnumerable<TExpected> Expected;
    }
}
