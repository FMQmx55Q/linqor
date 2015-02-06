using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly TKey _key;
        private readonly IReadOnlyList<TElement> _elements;

        public Grouping(TKey key, IReadOnlyList<TElement> elements)
        {
            _key = key;
            _elements = elements;
        }

        public TKey Key
        {
            get { return _key; }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
}
