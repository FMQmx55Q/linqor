using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the intersection of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Intersect<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right)
            where TKey : IComparable<TKey>
        {
            return left.Intersect(right, (l, r) => l.CompareTo(r));
        }

        /// <summary>
        /// Produces the intersection of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Intersect<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right, Func<TKey, TKey, int> compare)
        {
            return IntersectInternal(left, right, compare)
                .AsOrderedBy(left.KeySelector)
                .Distinct((l, r) => compare(l, r) == 0);
        }

        private static IEnumerable<T> IntersectInternal<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right, Func<TKey, TKey, int> compare)
        {
            using (var leftEnumerator = left.Source.GetEnumerator())
            using (var rightEnumerator = right.Source.GetEnumerator())
            {
                EnumeratorState<T> leftState = leftEnumerator.Next();
                EnumeratorState<T> rightState = rightEnumerator.Next();

                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    var leftKey = left.KeySelector(leftState.Current);
                    var rightKey = right.KeySelector(rightState.Current);
                    switch(compare(leftKey, rightKey))
                    {
                        case -1:
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            yield return leftState.Current;
                            foreach(T item in leftEnumerator.TakeWhile(
                                current => compare(left.KeySelector(current), rightKey) == 0,
                                last => leftState = last))
                                {
                                    yield return item;
                                }

                            yield return rightEnumerator.Current;
                            foreach(T item in rightEnumerator.TakeWhile(
                                current => compare(right.KeySelector(current), rightKey) == 0,
                                last => rightState = last))
                                {
                                    yield return item;
                                }
                            break;
                        case 1:
                            rightState = rightEnumerator.Next();
                            break;
                    }
                }
            }
        }
    }
}
