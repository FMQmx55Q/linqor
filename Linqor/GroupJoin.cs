using System;
using System.Collections.Generic;

namespace Linqor
{
    public static class GroupJoin
    {
        public static IEnumerable<TResult> OrderedGroupJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, IEnumerable<TInner>, TResult> resultSelector,
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
                    
                    yield return resultSelector(outerEnumerator.Current, innerGroup);
                    
                    TOuter previous = outerEnumerator.Current;
                    outerEnumerator.MoveNext();
                    
                    while(outerEnumerator.HasCurrent)
                    {
                        int compareResult = compare(outerKeySelector(outerEnumerator.Current), outerKeySelector(previous));
                        if (compareResult == 0)
                        {
                            yield return resultSelector(outerEnumerator.Current, innerGroup);
                            
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