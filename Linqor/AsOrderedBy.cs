using System;
using System.Collections.Generic;

namespace Linqor
{
    public class OrderedEnumerable<T, TKey>
    {
        public IEnumerable<T> Source { get; internal set; }
        public Func<T, TKey> KeySelector { get; internal set; }
    }

    public static partial class Extensions
    {
        public static OrderedEnumerable<T, TKey> AsOrderedBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return new OrderedEnumerable<T, TKey>
            {
                Source = source,
                KeySelector = keySelector
            };
        }
    }    
}