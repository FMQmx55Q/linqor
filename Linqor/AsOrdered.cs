using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static partial class Extensions
    {
        public static OrderedEnumerable<T, TKey> AsOrderedBy<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, Comparer<TKey>.Default, false);

        public static OrderedEnumerable<T, TKey> AsOrderedBy<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, keyComparer, false);

        public static OrderedEnumerable<T, TKey> AsOrderedByDescending<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, Comparer<TKey>.Default, true);

        public static OrderedEnumerable<T, TKey> AsOrderedByDescending<T, TKey>(
            this IEnumerable<T> source,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer) =>
            new OrderedEnumerable<T, TKey>(source, keySelector, keyComparer, true);

        internal static OrderedEnumerable<T, TKey> AsOrderedLike<T, TKey>(
            this IEnumerable<T> source,
            OrderedEnumerable<T, TKey> parent) =>
            new OrderedEnumerable<T, TKey>(
                source,
                parent.KeySelector,
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
                .AsOrderedLike(parent, group => group.Key);
        
        internal static OrderedEnumerable<U, TKey> AsOrderedLike<T, U, TKey>(
            this IEnumerable<U> source,
            OrderedEnumerable<T, TKey> parent,
            Func<U, TKey> keySelector) =>
            new OrderedEnumerable<U, TKey>(
                source,
                keySelector,
                parent.KeyComparer,
                parent.Descending);
    }    
}