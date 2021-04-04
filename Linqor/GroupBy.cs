using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Groups the elements of an ordered sequence.
        /// </summary>
        public static OrderedEnumerable<OrderedGrouping<TKey, T>, TKey> GroupBy<T, TKey>(
            this OrderedEnumerable<T, TKey> source)
        {
            return GroupBy(source, source.KeySelector, source.KeyComparer)
                .AsOrderedLike(source);
        }
        
        /// <summary>
        /// Groups the elements of an ordered sequence.
        /// </summary>
        private static IEnumerable<IGrouping<TKey, T>> GroupBy<T, TKey>(
            IEnumerable<T> source,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer)
        {
            using (var enumerator = source.GetEnumerator())
            {
                EnumeratorState<T> state = enumerator.Next();

                while (state.HasCurrent)
                {
                    TKey groupKey = keySelector(state.Current);
                    IReadOnlyList<T> elements = new[] { state.Current }
                        .Concat(enumerator
                            .TakeWhile(current => keyComparer.Compare(groupKey, keySelector(current)) == 0, last => state = last))
                        .ToArray();

                    yield return new Grouping<TKey, T>(groupKey, elements);
                }
            }
        }
    }
}
