using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Correlates the elements of two ordered sequences based on key equality and groups the results.
        /// Comparer from the first sequence is used to compare keys.
        /// </summary>
        /// <param name="outer">The first sequence to join.</param>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="resultSelector">
        /// A function to create a result element from an element from the first sequence
        /// and a collection of matching elements from the second sequence.
        /// </param>
        /// <param name="resultKeySelector">A function to extract key from a result element.</param>
        public static OrderedEnumerable<TResult, TKey> GroupJoin<TLeft, TRight, TKey, TResult>(
            this OrderedEnumerable<TLeft, TKey> outer,
            OrderedEnumerable<TRight, TKey> inner,
            Func<TLeft, OrderedEnumerable<TRight, TKey>, TResult> resultSelector,
            Func<TResult, TKey> resultKeySelector)
        {
            return GroupJoin(
                    outer,
                    inner,
                    outer.KeySelector,
                    inner.KeySelector,
                    (outerKey, innerSubset) => resultSelector(outerKey, innerSubset.AsOrderedLike(inner)),
                    outer.KeyComparer)
                .AsOrderedLike(resultKeySelector, outer);
        }

        /// <summary>
        /// Correlates the elements of two ordered sequences based on key equality, and groups the results.
        /// </summary>
        private static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(
            IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
            IComparer<TKey> keyComparer)
        {
            using (var leftEnumerator = outer.GetEnumerator())
            using (var rightEnumerator = inner.GetEnumerator())
            {
                EnumeratorState<TOuter> leftState = leftEnumerator.Next();
                EnumeratorState<TInner> rightState = rightEnumerator.Next();
                
                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    switch(keyComparer.Compare(outerKeySelector(leftState.Current), innerKeySelector(rightState.Current)))
                    {
                        case -1:
                            yield return resultSelector(leftState.Current, new TInner[] { });
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            TKey currentRightKey = innerKeySelector(rightState.Current);
                            IReadOnlyList<TInner> elements = new[] { rightState.Current }
                                .Concat(rightEnumerator
                                    .TakeWhile(current => keyComparer.Compare(currentRightKey, innerKeySelector(current)) == 0, last => rightState = last))
                                .ToArray();

                            yield return resultSelector(leftState.Current, elements);

                            TKey currentLeftKey = outerKeySelector(leftState.Current);
                            foreach (var leftItem in leftEnumerator.TakeWhile(current => keyComparer.Compare(currentLeftKey, outerKeySelector(current)) == 0, last => leftState = last))
                            {
                                yield return resultSelector(leftItem, elements);
                            }
                            break;
                        case 1:
                            rightState = rightEnumerator.Next();
                            break;
                    }
                }

                while (leftState.HasCurrent)
                {
                    yield return resultSelector(leftState.Current, new TInner[] { });
                    leftState = leftEnumerator.Next();
                }
            }         
        }
    }
}