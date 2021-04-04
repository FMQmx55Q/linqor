using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the intersection of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Intersect<T, TKey>(
            this OrderedEnumerable<T, TKey> left,
            OrderedEnumerable<T, TKey> right)
        {
            return Intersect(left, right, left.Comparer)
                .AsOrderedLike(left)
                .Distinct();                
        }

        private static IEnumerable<T> Intersect<T>(
            IEnumerable<T> left,
            IEnumerable<T> right,
            IComparer<T> comparer)
        {
            using (var leftEnumerator = left.GetEnumerator())
            using (var rightEnumerator = right.GetEnumerator())
            {
                EnumeratorState<T> leftState = leftEnumerator.Next();
                EnumeratorState<T> rightState = rightEnumerator.Next();

                while (leftState.HasCurrent && rightState.HasCurrent)
                {
                    switch(comparer.Compare(leftState.Current, rightState.Current))
                    {
                        case -1:
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            yield return leftState.Current;
                            foreach(T item in leftEnumerator.TakeWhile(
                                current => comparer.Compare(current, rightState.Current) == 0,
                                last => leftState = last))
                                {
                                    yield return item;
                                }

                            yield return rightEnumerator.Current;
                            foreach(T item in rightEnumerator.TakeWhile(
                                current => comparer.Compare(current, rightState.Current) == 0,
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
