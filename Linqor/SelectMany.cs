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
            return source
                .Select(selector)
                .Aggregate(seed, (left, right) => Extensions.Concat(left, right.AsOrderedLike(left)));
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
            Func<TSource, int, OrderedEnumerable<TResult, TResultKey>> selector)
        {
            return source
                .Select(selector)
                .Aggregate(seed, (left, right) => Extensions.Concat(left, right.AsOrderedLike(left)));
        }
        
    }
}