using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Produces the set intersection of two ordered sequences.
        /// </summary>
        /// <param name="left">
        /// An ordered sequence whose distinct elements
        /// that also appear in the second sequence will be returned.
        /// </param>
        /// <param name="right">
        /// A sequence that follows same ordering rules as the first sequence
        /// whose distinct elements
        /// that also appear in the first sequence will be returned.
        /// </param>
        public static IEnumerable<T> Intersect<T, TKey>(
            this OrderedEnumerable<T, TKey> left,
            IEnumerable<T> right)
        {
            return Intersect(left, right.AsOrderedLike(left), left.Comparer)
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
