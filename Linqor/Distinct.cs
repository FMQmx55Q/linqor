using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Distinct
    {
        /// <summary>
        /// Returns distinct elements from an ordered sequence.
        /// </summary>
        public static IEnumerable<T> OrderedDistinct<T>(this IEnumerable<T> source, Func<T, T, bool> equals)
        {
            using (var enumerator = new EnumeratorWrapper<T>(source.GetEnumerator()))
            {
                enumerator.MoveNext();

                if (!enumerator.HasCurrent)
                {
                    yield break;
                }

                yield return enumerator.Current;

                T previous = enumerator.Current;
                while (enumerator.HasCurrent)
                {
                    if (!equals(previous, enumerator.Current))
                    {
                        yield return enumerator.Current;
                    }
                    previous = enumerator.Current;

                    enumerator.MoveNext();
                }
            }
        }
    }
}
