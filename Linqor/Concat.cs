using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Concat
    {
        /// <summary>
        /// Concatenates two ordered sequences.
        /// </summary>
        public static IEnumerable<T> OrderedConcat<T>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, T, int> compare)
        {
            using (var leftEnumerator = left.GetEnumerator())
            using (var rightEnumerator = right.GetEnumerator())
            {
                EnumeratorState<T> leftState = leftEnumerator.Next();
                EnumeratorState<T> rightState = rightEnumerator.Next();

                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    switch(compare(leftState.Current, rightState.Current))
                    {
                        case -1:
                            yield return leftState.Current;
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            yield return leftState.Current;
                            foreach(T item in leftEnumerator.TakeWhile(current => compare(leftState.Current, current) == 0, last => leftState = last))
                            {
                                yield return item;
                            }

                            yield return rightEnumerator.Current;
                            foreach(T item in rightEnumerator.TakeWhile(current => compare(rightState.Current, current) == 0, last => rightState = last))
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
