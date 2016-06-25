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
            using (var enumerator = source.GetEnumerator())
            {
                EnumeratorState<T> state = enumerator.Next();

                while (state.HasCurrent)
                {
                    yield return state.Current;
                    state = enumerator.SkipWhile(current => equals(state.Current, current));
                }
            }
        }
    }
}
