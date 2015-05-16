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
            using (var enumerator = new EnumeratorWrapper<T>(source.GetEnumerator()))
            {
                enumerator.MoveNext();

                if (!enumerator.HasCurrent)
                {
                    yield break;
                }

                TKey groupKey = keySelector(enumerator.Current);
                List<T> elements = new List<T> { enumerator.Current };

                while (enumerator.MoveNext())
                {
                    TKey currentKey = keySelector(enumerator.Current);

                    if (equals(groupKey, currentKey))
                    {
                        elements.Add(enumerator.Current);
                    }
                    else
                    {
                        yield return new Grouping<TKey, T>(groupKey, elements);

                        groupKey = currentKey;
                        elements = new List<T> { enumerator.Current };
                    }
                }

                yield return new Grouping<TKey, T>(groupKey, elements);
            }
        }
    }
}
