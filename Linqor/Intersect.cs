using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Intersect
    {
        /// <summary>
        /// Produces the set intersection of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> OrderedIntersect<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, int> compare)
        {
            using (var firstEnumerator = new EnumeratorWrapper<T>(first.GetEnumerator()))
            using (var outerEnumerator = new EnumeratorWrapper<T>(second.GetEnumerator()))
            {
                firstEnumerator.MoveNext();
                outerEnumerator.MoveNext();

                while (firstEnumerator.HasCurrent && outerEnumerator.HasCurrent)
                {
                    int compareResult = compare(firstEnumerator.Current, outerEnumerator.Current);

                    if (compareResult == 0)
                    {
                        yield return firstEnumerator.Current;
                        firstEnumerator.MoveNext();
                        outerEnumerator.MoveNext();
                    }
                    else if (compareResult > 0)
                    {
                        outerEnumerator.MoveNext();
                    }
                    else
                    {
                        firstEnumerator.MoveNext();
                    }
                }
            }
        }
    }
}
