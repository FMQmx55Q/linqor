using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Concat
    {
        /// <summary>
        /// Concatenates two ordered sequences.
        /// </summary>
        public static IEnumerable<T> OrderedConcat<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, int> compare)
        {
            using (var firstEnumerator = new EnumeratorWrapper<T>(first.GetEnumerator()))
            using (var secondEnumerator = new EnumeratorWrapper<T>(second.GetEnumerator()))
            {
                firstEnumerator.MoveNext();
                secondEnumerator.MoveNext();

                while (firstEnumerator.HasCurrent && secondEnumerator.HasCurrent)
                {
                    int compareResult = compare(firstEnumerator.Current, secondEnumerator.Current);
                    if (compareResult == 0)
                    {
                        yield return firstEnumerator.Current;
                        firstEnumerator.MoveNext();

                        yield return secondEnumerator.Current;
                        secondEnumerator.MoveNext();
                    }
                    else if (compareResult > 0)
                    {
                        yield return secondEnumerator.Current;
                        secondEnumerator.MoveNext();
                    }
                    else
                    {
                        yield return firstEnumerator.Current;
                        firstEnumerator.MoveNext();
                    }
                }

                while (firstEnumerator.HasCurrent)
                {
                    yield return firstEnumerator.Current;
                    firstEnumerator.MoveNext();
                }

                while (secondEnumerator.HasCurrent)
                {
                    yield return secondEnumerator.Current;
                    secondEnumerator.MoveNext();
                }
            }
        }
    }
}
