using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the set difference of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Except<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right)
            where TKey : IComparable<TKey>
        {
            return left.Except(right, (l, r) => l.CompareTo(r));
        }
        
        /// <summary>
        /// Produces the set difference of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Except<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right, Func<TKey, TKey, int> compare)
        {
            Func<TKey, TKey, bool> equals = (l, r) => compare(l, r) == 0; 
            using (var leftEnumerator = left.Distinct(equals).GetEnumerator())
            using (var rightEnumerator = right.Distinct(equals).GetEnumerator())
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
