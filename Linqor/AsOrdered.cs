using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Wraps a sequence ordered in ascending order according to a key.
        /// </summary>
        /// <param name="source">An ordered sequence of values.</param>
        /// <param name="keySelector">A function to extract key from an element.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        public static OrderedEnumerable<T, TKey> AsOrderedBy<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, Comparer<TKey>.Default, false);

        /// <summary>
        /// Wraps a sequence ordered in ascending order by using a specified comparer.
        /// </summary>
        /// <param name="source">An ordered sequence of values.</param>
        /// <param name="keySelector">A function to extract key from an element.</param>
        /// <param name="keyComparer">An System.Collections.Generic.IComparer`1 to compare keys.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        public static OrderedEnumerable<T, TKey> AsOrderedBy<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, keyComparer, false);

        /// <summary>
        /// Wraps a sequence ordered in descending order according to a key.
        /// </summary>
        /// <param name="source">An ordered sequence of values.</param>
        /// <param name="keySelector">A function to extract key from an element.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        public static OrderedEnumerable<T, TKey> AsOrderedByDescending<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, Comparer<TKey>.Default, true);

        /// <summary>
        /// Wraps a sequence ordered in descending order by using a specified comparer.
        /// </summary>
        /// <param name="source">An ordered sequence of values.</param>
        /// <param name="keySelector">A function to extract key from an element.</param>
        /// <param name="keyComparer">An System.Collections.Generic.IComparer`1 to compare keys.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        public static OrderedEnumerable<T, TKey> AsOrderedByDescending<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, keyComparer, true);

        internal static OrderedEnumerable<T, TKey> AsOrderedLike<T, TKey>(
            this IEnumerable<T> source,
            OrderedEnumerable<T, TKey> parent) =>
            AsOrderedLike(source, parent.KeySelector, parent);
        
        internal static OrderedEnumerable<U, TKey> AsOrderedLike<T, U, TKey>(
            this IEnumerable<U> source,
            Func<U, TKey> keySelector,
            OrderedEnumerable<T, TKey> parent) =>
            new OrderedEnumerable<U, TKey>(
                source,
                keySelector,
                parent.KeyComparer,
                parent.Descending);

        internal static OrderedEnumerable<OrderedGrouping<TKey, T>, TKey> AsOrderedLike<T, TKey>(
            this IEnumerable<IGrouping<TKey, T>> source,
            OrderedEnumerable<T, TKey> parent) =>
            source
                .Select(group => new OrderedGrouping<TKey, T>(
                    group,
                    parent.KeySelector,
                    parent.KeyComparer,
                    parent.Descending))
                .AsOrderedLike(group => group.Key, parent);
    }
}