using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Except
    {
        /// <summary>
        /// Produces the set difference of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> OrderedExcept<T>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            using (var outerEnumerator = new EnumeratorWrapper<T>(outer.GetEnumerator()))
            using (var innerEnumerator = new EnumeratorWrapper<T>(inner.GetEnumerator()))
            {
                outerEnumerator.MoveNext();
                innerEnumerator.MoveNext();

                while (outerEnumerator.HasCurrent && innerEnumerator.HasCurrent)
                {
                    int compareResult = compare(outerEnumerator.Current, innerEnumerator.Current);

                    if (compareResult < 0)
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();
                    }
                    else if (compareResult > 0)
                    {
                        yield return outerEnumerator.Current;
                        innerEnumerator.MoveNext();
                    }
                    else
                    {
                        outerEnumerator.MoveNext();
                        innerEnumerator.MoveNext();
                    }
                }
            }
        }
    }
}
