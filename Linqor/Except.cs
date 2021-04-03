using System;
using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the difference of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Except<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right)
            where TKey : IComparable<TKey>
        {
            return left.Except(right, (l, r) => l.CompareTo(r));
        }
        
        /// <summary>
        /// Produces the difference of two ordered sequences.
        /// </summary>
        public static IEnumerable<T> Except<T, TKey>(this OrderedEnumerable<T, TKey> left, OrderedEnumerable<T, TKey> right, Func<TKey, TKey, int> compare)
        {
            left = left.Distinct((l, r) => compare(l, r) == 0).AsOrderedBy(left.KeySelector);
            right = right.Distinct((l, r) => compare(l, r) == 0).AsOrderedBy(right.KeySelector);

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
                            var leftKey = left.KeySelector(leftState.Current);
                            leftState = leftEnumerator.SkipWhile(current => compare(leftKey, left.KeySelector(current)) == 0);
                            
                            var rightKey = right.KeySelector(rightState.Current);
                            rightState = rightEnumerator.SkipWhile(current => compare(rightKey, right.KeySelector(current)) == 0);
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
