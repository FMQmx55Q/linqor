using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Union
    {
        /// <summary>
        /// Produces the set union of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> OrderedUnion<T>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            using (var outerEnumerator = new EnumeratorWrapper<T>(outer.GetEnumerator()))
            using (var innerEnumerator = new EnumeratorWrapper<T>(inner.GetEnumerator()))
            {
                outerEnumerator.MoveNext();
                innerEnumerator.MoveNext();

                while (outerEnumerator.HasCurrent && innerEnumerator.HasCurrent)
                {
                    int compareResult = compare(outerEnumerator.Current, innerEnumerator.Current);
                    if (compareResult > 0)
                    {
                        yield return innerEnumerator.Current;
                        innerEnumerator.MoveNext();
                    }
                    else if (compareResult == 0)
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();
                        innerEnumerator.MoveNext();
                    }
                    else
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();
                    }
                }

                while (outerEnumerator.HasCurrent)
                {
                    yield return outerEnumerator.Current;
                    outerEnumerator.MoveNext();
                }

                while (innerEnumerator.HasCurrent)
                {
                    yield return innerEnumerator.Current;
                    innerEnumerator.MoveNext();
                }
            }
        }
    }
}
