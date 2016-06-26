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
        public static IEnumerable<IGrouping<TKey, T>> GroupBy<T, TKey>(this OrderedEnumerable<T, TKey> source, Func<TKey, TKey, bool> equals)
        {
            using (var enumerator = source.Source.GetEnumerator())
            {
                EnumeratorState<T> state = enumerator.Next();

                while (state.HasCurrent)
                {
                    TKey groupKey = source.KeySelector(state.Current);
                    IReadOnlyList<T> elements = new[] { state.Current }
                        .Concat(enumerator
                            .TakeWhile(current => equals(groupKey, source.KeySelector(current)), last => state = last))
                        .ToArray();

                    yield return new Grouping<TKey, T>(groupKey, elements);
                }
            }
        }
    }
}
