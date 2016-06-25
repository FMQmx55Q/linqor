using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static class GroupBy
    {
        /// <summary>
        /// Groups the elements of an ordered sequence.
        /// </summary>
        public static IEnumerable<IGrouping<TKey, T>> OrderedGroupBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<TKey, TKey, bool> equals)
        {
            using (var enumerator = source.GetEnumerator())
            {
                EnumeratorState<T> state = enumerator.Next();

                while (state.HasCurrent)
                {
                    TKey groupKey = keySelector(state.Current);
                    IReadOnlyList<T> elements = new[] { state.Current }
                        .Concat(enumerator
                            .TakeWhile(current => equals(groupKey, keySelector(current)), last => state = last))
                        .ToArray();

                    yield return new Grouping<TKey, T>(groupKey, elements);
                }
            }
        }
    }
}
