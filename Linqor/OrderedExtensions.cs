using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static class OrderedExtensions
    {
        public static IEnumerable<T> OrderedDistinct<T>(this IEnumerable<T> source)
            where T : IEquatable<T>
        {
            return OrderedDistinct(source, (t1, t2) => t1.Equals(t2));
        }

        public static IEnumerable<T> OrderedDistinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
            where TKey : IEquatable<TKey>
        {
            return OrderedDistinct(source, (t1, t2) => keySelector(t1).Equals(keySelector(t2)));
        }

        public static IEnumerable<T> OrderedDistinct<T>(this IEnumerable<T> source, Func<T, T, bool> equals)
        {
            using (var enumerator = new EnumeratorWrapper<T>(source.GetEnumerator()))
            {
                enumerator.MoveNext();

                if (!enumerator.HasCurrent)
                {
                    yield break;
                }

                yield return enumerator.Current;

                T previous = enumerator.Current;
                while (enumerator.HasCurrent)
                {
                    if (!equals(previous, enumerator.Current))
                    {
                        yield return enumerator.Current;
                    }
                    previous = enumerator.Current;

                    enumerator.MoveNext();
                }
            }
        }

        public static IEnumerable<T> OrderedConcat<T>(this IEnumerable<T> outer, IEnumerable<T> inner)
            where T : IComparable<T>
        {
            return OrderedConcat(outer, inner, (t1, t2) => t1.CompareTo(t2));
        }

        public static IEnumerable<T> OrderedConcat<T, TKey>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            return OrderedConcat(outer, inner, (t1, t2) => keySelector(t1).CompareTo(keySelector(t2)));
        }

        public static IEnumerable<T> OrderedConcat<T>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            using (var outerEnumerator = new EnumeratorWrapper<T>(outer.GetEnumerator()))
            using (var innerEnumerator = new EnumeratorWrapper<T>(inner.GetEnumerator()))
            {
                outerEnumerator.MoveNext();
                innerEnumerator.MoveNext();

                while (outerEnumerator.HasCurrent && innerEnumerator.HasCurrent)
                {
                    int compareResult = compare(outerEnumerator.Current, innerEnumerator.Current);
                    if (compareResult == 0)
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();

                        yield return innerEnumerator.Current;
                        innerEnumerator.MoveNext();
                    }
                    else if (compareResult > 0)
                    {
                        yield return innerEnumerator.Current;
                        innerEnumerator.MoveNext();
                    }
                    else
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();
                    }
                }

                while (outerEnumerator.HasCurrent)
                {
                    yield return outerEnumerator.Current;
                    outerEnumerator.MoveNext();
                }

                while (innerEnumerator.HasCurrent)
                {
                    yield return innerEnumerator.Current;
                    innerEnumerator.MoveNext();
                }
            }
        }

        public static IEnumerable<IGrouping<TKey, T>> OrderedGroupBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
            where TKey : IEquatable<TKey>
        {
            return OrderedGroupBy(source, keySelector, (t1, t2) => t1.Equals(t2));
        }

        public static IEnumerable<IGrouping<TKey, T>> OrderedGroupBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<TKey, TKey, bool> equals)
        {
            using (var enumerator = new EnumeratorWrapper<T>(source.GetEnumerator()))
            {
                enumerator.MoveNext();

                if (!enumerator.HasCurrent)
                {
                    yield break;
                }

                TKey groupKey = keySelector(enumerator.Current);
                List<T> elements = new List<T> { enumerator.Current };

                while (enumerator.MoveNext())
                {
                    TKey currentKey = keySelector(enumerator.Current);

                    if (equals(groupKey, currentKey))
                    {
                        elements.Add(enumerator.Current);
                    }
                    else
                    {
                        yield return new Grouping<TKey, T>(groupKey, elements);

                        groupKey = currentKey;
                        elements = new List<T> { enumerator.Current };
                    }
                }

                yield return new Grouping<TKey, T>(groupKey, elements);
            }
        }

        public static IEnumerable<TResult> OrderedJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
            where TKey : IComparable<TKey>
        {
            using (var outerEnumerator = new EnumeratorWrapper<TOuter>(outer.GetEnumerator()))
            using (var innerEnumerator = new EnumeratorWrapper<TInner>(inner.GetEnumerator()))
            {
                outerEnumerator.MoveNext();
                innerEnumerator.MoveNext();

                while (outerEnumerator.HasCurrent && innerEnumerator.HasCurrent)
                {
                    int compareResult = outerKeySelector(outerEnumerator.Current).CompareTo(innerKeySelector(innerEnumerator.Current));
                    if (compareResult == 0)
                    {
                        yield return resultSelector(outerEnumerator.Current, innerEnumerator.Current);

                        outerEnumerator.MoveNext();
                        innerEnumerator.MoveNext();
                    }
                    else if (compareResult > 0)
                    {
                        innerEnumerator.MoveNext();
                    }
                    else
                    {
                        outerEnumerator.MoveNext();
                    }
                }
            }
        }

        public static IEnumerable<T> OrderedUnion<T>(this IEnumerable<T> outer, IEnumerable<T> inner)
            where T : IComparable<T>
        {
            return OrderedUnion(outer, inner, (t1, t2) => t1.CompareTo(t2));
        }

        public static IEnumerable<T> OrderedUnion<T, TKey>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            return OrderedUnion(outer, inner, (t1, t2) => keySelector(t1).CompareTo(keySelector(t2)));
        }

        public static IEnumerable<T> OrderedUnion<T>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            using (var outerEnumerator = new EnumeratorWrapper<T>(outer.GetEnumerator()))
            using (var innerEnumerator = new EnumeratorWrapper<T>(inner.GetEnumerator()))
            {
                outerEnumerator.MoveNext();
                innerEnumerator.MoveNext();

                while (outerEnumerator.HasCurrent && innerEnumerator.HasCurrent)
                {
                    int compareResult = compare(outerEnumerator.Current, innerEnumerator.Current);
                    if (compareResult > 0)
                    {
                        yield return innerEnumerator.Current;
                        innerEnumerator.MoveNext();
                    }
                    else if (compareResult == 0)
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();
                        innerEnumerator.MoveNext();
                    }
                    else
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();
                    }
                }

                while (outerEnumerator.HasCurrent)
                {
                    yield return outerEnumerator.Current;
                    outerEnumerator.MoveNext();
                }

                while (innerEnumerator.HasCurrent)
                {
                    yield return innerEnumerator.Current;
                    innerEnumerator.MoveNext();
                }
            }
        }

        public static IEnumerable<T> OrderedIntersect<T>(this IEnumerable<T> outer, IEnumerable<T> inner)
            where T : IComparable<T>
        {
            return OrderedIntersect(outer, inner, (t1, t2) => t1.CompareTo(t2));
        }

        public static IEnumerable<T> OrderedIntersect<T, TKey>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            return OrderedIntersect(outer, inner, (t1, t2) => keySelector(t1).CompareTo(keySelector(t2)));
        }

        public static IEnumerable<T> OrderedIntersect<T>(this IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare)
        {
            using (var outerEnumerator = new EnumeratorWrapper<T>(outer.GetEnumerator()))
            using (var innerEnumerator = new EnumeratorWrapper<T>(inner.GetEnumerator()))
            {
                outerEnumerator.MoveNext();
                innerEnumerator.MoveNext();

                while (outerEnumerator.HasCurrent && innerEnumerator.HasCurrent)
                {
                    int compareResult = compare(outerEnumerator.Current, innerEnumerator.Current);

                    if (compareResult == 0)
                    {
                        yield return outerEnumerator.Current;
                        outerEnumerator.MoveNext();
                        innerEnumerator.MoveNext();
                    }
                    else if (compareResult > 0)
                    {
                        innerEnumerator.MoveNext();
                    }
                    else
                    {
                        outerEnumerator.MoveNext();
                    }
                }
            }
        }
    }
}
