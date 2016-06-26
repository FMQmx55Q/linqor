using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns distinct elements from an ordered sequence.
        /// </summary>
        public static IEnumerable<T> Distinct<T, TKey>(this OrderedEnumerable<T, TKey> source, Func<TKey, TKey, bool> equals)
        {
            using (var enumerator = source.Source.GetEnumerator())
            {
                EnumeratorState<T> state = enumerator.Next();

                while (state.HasCurrent)
                {
                    yield return state.Current;
                    state = enumerator.SkipWhile(current => equals(source.KeySelector(state.Current), source.KeySelector(current)));
                }
            }
        }
    }
}
