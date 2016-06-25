using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static class GroupJoin
    {
        public static IEnumerable<TResult> OrderedGroupJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<TLeft, IEnumerable<TRight>, TResult> resultSelector,
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
                            yield return resultSelector(leftState.Current, new TRight[] { });
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            TKey currentRightKey = rightKeySelector(rightState.Current);
                            IReadOnlyList<TRight> elements = new[] { rightState.Current }
                                .Concat(rightEnumerator
                                    .TakeWhile(current => compare(currentRightKey, rightKeySelector(current)) == 0, last => rightState = last))
                                .ToArray();

                            yield return resultSelector(leftState.Current, elements);

                            TKey currentLeftKey = leftKeySelector(leftState.Current);
                            foreach (var leftItem in leftEnumerator.TakeWhile(current => compare(currentLeftKey, leftKeySelector(current)) == 0, last => leftState = last))
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
                    yield return resultSelector(leftState.Current, new TRight[] { });
                    leftState = leftEnumerator.Next();
                }
            }         
        }
    }
}