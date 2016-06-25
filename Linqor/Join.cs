using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Join
    {
        /// <summary>
        /// Correlates the elements of two ordered sequences based on matching keys.
        /// </summary>
        public static IEnumerable<TResult> OrderedJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> resultSelector,
            Func<TKey, TKey, int> compare)
        {
            using (var leftEnumerator = left.GetEnumerator())
            using (var rightEnumerator = right.GetEnumerator())
            {
                EnumeratorState<TLeft> leftState = leftEnumerator.Next();
                EnumeratorState<TRight> rightState = rightEnumerator.Next();
                
                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    switch(compare(leftKeySelector(leftState.Current), rightKeySelector(rightState.Current)))
                    {
                        case -1:
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            yield return resultSelector(leftState.Current, rightState.Current);

                            List<TRight> rightGroup = new List<TRight> { rightState.Current };

                            TKey currentRightKey = rightKeySelector(rightState.Current);
                            foreach(TRight rightItem in rightEnumerator.TakeWhile(current => compare(currentRightKey, rightKeySelector(current)) == 0, last => rightState = last))
                            {
                                rightGroup.Add(rightItem);
                                yield return resultSelector(leftState.Current, rightItem);
                            }

                            TKey currentLeftKey = leftKeySelector(leftState.Current);
                            foreach (var leftItem in leftEnumerator.TakeWhile(current => compare(currentLeftKey, leftKeySelector(current)) == 0, last => leftState = last))
                            {
                                foreach(TRight rightItem in rightGroup)
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
