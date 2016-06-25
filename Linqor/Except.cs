using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Except
    {
        /// <summary>
        /// Produces the set difference of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> OrderedExcept<T>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, T, int> compare)
        {
            Func<T, T, bool> equals = (l, r) => compare(l, r) == 0; 
            using (var leftEnumerator = left.OrderedDistinct(equals).GetEnumerator())
            using (var rightEnumerator = right.OrderedDistinct(equals).GetEnumerator())
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
                            leftState = leftEnumerator.Next();
                            rightState = rightEnumerator.Next();
                            break;
                        case 1:
                            rightState = rightEnumerator.Next();
                            break;
                    };
                }

                while(leftState.HasCurrent)
                {
                    yield return leftState.Current;
                    leftState = leftEnumerator.Next();
                }
            }
        }
    }
}
