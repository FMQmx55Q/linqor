using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly TKey _key;
        private readonly IEnumerable<TElement> _elements;

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            _key = key;
            _elements = elements;
        }

        public TKey Key => _key;

        public IEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
