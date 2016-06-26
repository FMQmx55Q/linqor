using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Concatenates two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Concat<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right, Func<TKey, TKey, int> compare)
        {
            using (var leftEnumerator = left.Source.GetEnumerator())
            using (var rightEnumerator = right.Source.GetEnumerator())
            {
                EnumeratorState<T> leftState = leftEnumerator.Next();
                EnumeratorState<T> rightState = rightEnumerator.Next();

                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    switch(compare(left.KeySelector(leftState.Current), right.KeySelector(rightState.Current)))
                    {
                        case -1:
                            yield return leftState.Current;
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            yield return leftState.Current;
                            foreach(T item in leftEnumerator.TakeWhile(current => compare(left.KeySelector(leftState.Current), left.KeySelector(current)) == 0, last => leftState = last))
                            {
                                yield return item;
                            }

                            yield return rightEnumerator.Current;
                            foreach(T item in rightEnumerator.TakeWhile(current => compare(right.KeySelector(rightState.Current), right.KeySelector(current)) == 0, last => rightState = last))
                            {
                                yield return item;
                            }
                            break;
                        case 1:
                            yield return rightEnumerator.Current;
                            rightState = rightEnumerator.Next();
                            break;
                    }
                }

                while (leftState.HasCurrent)
                {
                    yield return leftState.Current;
                    leftState = leftEnumerator.Next();
                }

                while (rightState.HasCurrent)
                {
                    yield return rightState.Current;
                    rightState = rightEnumerator.Next();
                }
            }
        }
    }
}
