using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static partial class Extensions
    {
        public static IEnumerable<TResult> GroupJoin<TLeft, TRight, TKey, TResult>(
            this OrderedEnumerable<TLeft, TKey> left,
            OrderedEnumerable<TRight, TKey> right,
            Func<TLeft, IReadOnlyList<TRight>, TResult> resultSelector,
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
                            yield return resultSelector(leftState.Current, new TRight[] { });
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            TKey currentRightKey = right.KeySelector(rightState.Current);
                            IReadOnlyList<TRight> elements = new[] { rightState.Current }
                                .Concat(rightEnumerator
                                    .TakeWhile(current => compare(currentRightKey, right.KeySelector(current)) == 0, last => rightState = last))
                                .ToArray();

                            yield return resultSelector(leftState.Current, elements);

                            TKey currentLeftKey = left.KeySelector(leftState.Current);
                            foreach (var leftItem in leftEnumerator.TakeWhile(current => compare(currentLeftKey, left.KeySelector(current)) == 0, last => leftState = last))
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