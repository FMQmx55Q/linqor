using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Union
    {
        /// <summary>
        /// Produces the set union of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> OrderedUnion<T>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, T, int> compare)
        {
            Func<T, T, bool> equals = (l, r) => compare(l, r) == 0; 
            using (var leftEnumerator = left.OrderedDistinct(equals).GetEnumerator())
            using (var rightEnumerator = right.OrderedDistinct(equals).GetEnumerator())
            {
                EnumeratorState<T> leftState = leftEnumerator.Next();
                EnumeratorState<T> rightState = rightEnumerator.Next();

                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    switch(compare(leftEnumerator.Current, rightEnumerator.Current))
                    {
                        case -1:
                            yield return leftEnumerator.Current;
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            yield return leftEnumerator.Current;
                            leftState = leftEnumerator.Next();
                            rightState = rightEnumerator.Next();
                            break;
                        case 1:
                            yield return rightEnumerator.Current;
                            rightState = rightEnumerator.Next();
                            break;
                    }
                }

                while (leftState.HasCurrent)
                {
                    yield return leftEnumerator.Current;
                    leftState = leftEnumerator.Next();
                }

                while (rightState.HasCurrent)
                {
                    yield return rightEnumerator.Current;
                    rightState = rightEnumerator.Next();
                }
            }
        }
    }
}
