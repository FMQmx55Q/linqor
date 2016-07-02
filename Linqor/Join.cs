using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Correlates the elements of two ordered sequences based on matching keys.
        /// </summary>
        public static IEnumerable<TResult> Join<TLeft, TRight, TKey, TResult>(
            this OrderedEnumerable<TLeft, TKey> left,
            OrderedEnumerable<TRight, TKey> right,
            Func<TLeft, TRight, TResult> resultSelector)
            where TKey : IComparable<TKey>
        {
            return left.Join(right, resultSelector, (l, r) => l.CompareTo(r));
        }
        
        /// <summary>
        /// Correlates the elements of two ordered sequences based on matching keys.
        /// </summary>
        public static IEnumerable<TResult> Join<TLeft, TRight, TKey, TResult>(
            this OrderedEnumerable<TLeft, TKey> left,
            OrderedEnumerable<TRight, TKey> right,
            Func<TLeft, TRight, TResult> resultSelector,
            Func<TKey, TKey, int> compare)
        {
            using (var leftEnumerator = left.Source.GetEnumerator())
            using (var rightEnumerator = right.Source.GetEnumerator())
            {
                EnumeratorState<TLeft> leftState = leftEnumerator.Next();
                EnumeratorState<TRight> rightState = rightEnumerator.Next();
                
                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    switch(compare(left.KeySelector(leftState.Current), right.KeySelector(rightState.Current)))
                    {
                        case -1:
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            yield return resultSelector(leftState.Current, rightState.Current);

                            TKey currentRightKey = right.KeySelector(rightState.Current);
                            List<TRight> elements = new List<TRight> { rightState.Current };
                            foreach(TRight rightItem in rightEnumerator.TakeWhile(current => compare(currentRightKey, right.KeySelector(current)) == 0, last => rightState = last))
                            {
                                elements.Add(rightItem);
                                yield return resultSelector(leftState.Current, rightItem);
                            }

                            TKey currentLeftKey = left.KeySelector(leftState.Current);
                            foreach (var leftItem in leftEnumerator.TakeWhile(current => compare(currentLeftKey, left.KeySelector(current)) == 0, last => leftState = last))
                            {
                                foreach(TRight rightItem in elements)
                                {
                                    yield return resultSelector(leftItem, rightItem);
                                }
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
