using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the difference of two ordered sequences.
        /// </summary>
        public static OrderedEnumerable<T, TKey> Except<T, TKey>(
            this OrderedEnumerable<T, TKey> left,
            IEnumerable<T> right)
        {
            return Except(left, right.AsOrderedLike(left), left.Comparer)
                .AsOrderedLike(left)
                .Distinct();
        }

        private static IEnumerable<T> Except<T>(
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
                            yield return leftState.Current;
                            leftState = leftEnumerator.Next();
                            break;
                        case 0:
                            leftState = leftEnumerator.SkipWhile(current => comparer.Compare(leftState.Current, current) == 0);
                            rightState = rightEnumerator.SkipWhile(current => comparer.Compare(rightState.Current, current) == 0);
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
