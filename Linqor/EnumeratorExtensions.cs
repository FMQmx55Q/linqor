using System;
using System.Collections.Generic;

namespace Linqor
{
    internal struct EnumeratorState<T>
    {
        public bool HasCurrent { get; set; }
        public T Current { get; set; }
    }
    internal static class EnumeratorExtensions
    {
        internal static EnumeratorState<T> SkipWhile<T>(this IEnumerator<T> enumerator, Func<T, bool> predicate)
        {
            EnumeratorState<T> state;
            while((state = enumerator.Next()).HasCurrent && predicate(state.Current)) { };
            return state;
        }

        internal static IEnumerable<T> TakeWhile<T>(this IEnumerator<T> enumerator, Func<T, bool> predicate, Action<EnumeratorState<T>> onLast)
        {
            EnumeratorState<T> state;
            while((state = enumerator.Next()).HasCurrent && predicate(state.Current))
            {
                yield return state.Current;
            }
            onLast(state);
        }

        internal static EnumeratorState<T> Next<T>(this IEnumerator<T> enumerator)
        {
            bool hasCurrent = enumerator.MoveNext(); 
            return new EnumeratorState<T> { 
                HasCurrent = hasCurrent,
                Current = hasCurrent
                    ? enumerator.Current
                    : default(T)
            };
        }
    }
}
