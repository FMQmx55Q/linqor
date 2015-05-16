﻿using System;
using System.Collections.Generic;

namespace Linqor.Tests
{
    public static class BinaryTestCase
    {
        public static Func<IEnumerable<T>, IEnumerable<T>, IEnumerable<T>, BinaryTestCase<T>> GetCreator<T>(string name)
        {
            return (outer, inner, expected) => BinaryTestCase.Create<T>(name, outer, inner, expected);
        }

        public static BinaryTestCase<T> Create<T>(string name, IEnumerable<T> outer, IEnumerable<T> inner, IEnumerable<T> expected)
        {
            return new BinaryTestCase<T>
            {
                Name = name,
                Outer = outer,
                Inner = inner,
                Expected = expected
            };
        }
    }
    public class BinaryTestCase<T>
    {
        public string Name;
        public IEnumerable<T> Outer;
        public IEnumerable<T> Inner;
        public IEnumerable<T> Expected;
    }
}
