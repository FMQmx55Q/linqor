using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class Join
    {
        /// <summary>
        /// Correlates the elements of two ordered sequences based on matching keys.
        /// </summary>
        public static IEnumerable<TResult> OrderedJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            Func<TKey, TKey, int> compare)
        {
            using (var outerEnumerator = new EnumeratorWrapper<TOuter>(outer.GetEnumerator()))
            using (var innerEnumerator = new EnumeratorWrapper<TInner>(inner.GetEnumerator()))
            {
                outerEnumerator.MoveNext();
                innerEnumerator.MoveNext();
                
                while (outerEnumerator.HasCurrent)
                {
                    List<TInner> innerGroup = new List<TInner>();
                    
                    while(innerEnumerator.HasCurrent)
                    {
                        int compareResult = compare(outerKeySelector(outerEnumerator.Current), innerKeySelector(innerEnumerator.Current));
                        if (compareResult == 0)
                        {
                            innerGroup.Add(innerEnumerator.Current);
                            
                            yield return resultSelector(outerEnumerator.Current, innerEnumerator.Current);                            

                            innerEnumerator.MoveNext();
                        }
                        else if (compareResult > 0)
                        {
                            innerEnumerator.MoveNext();
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    TOuter previous = outerEnumerator.Current;
                    outerEnumerator.MoveNext();
                    
                    while(outerEnumerator.HasCurrent)
                    {
                        int compareResult = compare(outerKeySelector(outerEnumerator.Current), outerKeySelector(previous));
                        if (compareResult == 0)
                        {
                            foreach (TInner item in innerGroup)
                            {
                                yield return resultSelector(outerEnumerator.Current, item);
                            }
                            
                            previous = outerEnumerator.Current;
                            outerEnumerator.MoveNext();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
