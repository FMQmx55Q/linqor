using System.Collections.Generic;

namespace Linqor.Tests
{
    public static class UnaryTestCase
    {
        public static UnaryTestCase<T> Create<T>(string name, IEnumerable<T> source, IEnumerable<T> expected)
        {
            return new UnaryTestCase<T>
            {
                Name = name,
                Source = source,
                Expected = expected
            };
        }
    }

    public class UnaryTestCase<T>
    {
        public string Name;
        public IEnumerable<T> Source;
        public IEnumerable<T> Expected;
    }
}
