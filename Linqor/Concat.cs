using System.Collections.Generic;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Concatenates two ordered sequences.
        /// </summary>
        public static OrderedEnumerable<T, TKey> Concat<T, TKey>(
            this OrderedEnumerable<T, TKey> left,
            IEnumerable<T> right)
        {
            return Concat(left, right.AsOrderedLike(left), left.Comparer)
                .AsOrderedLike(left);
        }

        private static IEnumerable<T> Concat<T>(
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
                            yield return rightState.Current;
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
