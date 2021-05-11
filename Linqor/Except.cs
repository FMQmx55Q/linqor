using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the set difference of two ordered sequences.
        /// </summary>
        /// <param name="left">
        /// An ordered sequence whose elements
        /// that are not also in second will be returned.
        /// </param>
        /// <param name="right">
        /// An ordered sequence that follows same ordering rules as the first ordered sequence
        /// whose elements that also occur in the first ordered sequence
        /// will cause those elements to be removed from the returned sequence.
        /// </param>
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
