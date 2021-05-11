using System;
using System.Collections.Generic;
using System.Linq;

namespace Linqor
{
    public static partial class Extensions
    {
        /// <summary>
        /// Projects each element of an ordered sequence
        /// and flattens the resulting ordered sequences into one ordered sequence.
        /// </summary>
        /// <param name="source">An ordered sequence of values to project.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        public static OrderedEnumerable<TResult, TResultKey> SelectMany<TSource, TSourceKey, TResult, TResultKey>(
            this OrderedEnumerable<TSource, TSourceKey> source,
            OrderedEnumerable<TResult, TResultKey> seed,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            return SelectMany(source, (item, _) => selector(item), seed.Comparer)
                .AsOrderedLike(seed);
        }

        /// <summary>
        /// Projects each element of an ordered sequence
        /// and flattens the resulting ordered sequences into one ordered sequence. The index of each source
        /// element is used in the projected form of that element.
        /// </summary>
        /// <param name="source">An ordered sequence of values to project.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the element.</param>
        public static OrderedEnumerable<TResult, TResultKey> SelectMany<TSource, TSourceKey, TResult, TResultKey>(
            this OrderedEnumerable<TSource, TSourceKey> source,
            OrderedEnumerable<TResult, TResultKey> seed,
            Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return SelectMany(source, selector, seed.Comparer)
                .AsOrderedLike(seed);
        }

        private static IEnumerable<TResult> SelectMany<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, int, IEnumerable<TResult>> selector,
            IComparer<TResult> comparer)
        {
            var result = Enumerable.Empty<TResult>();

            foreach (var items in source.Select(selector))
            {
                result = Extensions.Concat(result, items, comparer);
            }
            
            foreach (var item in result)
            {
                yield return item;
            }
        }
    }
}