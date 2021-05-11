# Linqor
[![Build Status](https://travis-ci.org/dangerozov/linqor.svg?branch=master)](https://travis-ci.org/dangerozov/linqor)

Linq extensions for ordered sequences. They have better perfomance in comparison to standard extensions. They don't enumerate entire sequences into `HashSet<T>` or lookup. With these you can lazily handle very large or even infinite sequences as long as they're ordered.

**Concat**, **Distinct**, **Except**, **GroupBy**, **GroupJoin**, **Intersect**, **Join**, **SelectMany** and **Union** are available.

## Usage
Cast `IEnumerable<T>` to `OrderedEnumberable<T, TKey>` by calling `AsOrderedBy<T, TKey>`. Use extensions as usual.

```csharp
new[] { 1, 1, 2, 2, 2 }.AsOrderedBy(x => x).Distinct(); // [ 1, 2 ]
new[] { 1, 3, 5 }.AsOrderedBy(x => x).Concat(new[] { 2, 4, 6 }.AsOrderedBy(x => x)); // [ 1, 2, 3, 4, 5, 6 ]
```

## Difference to LINQ API
No `IComparer<T> comparer` or `Func<T, TKey> keySelector` parameters. Ones from `OrderedEnumerable<T, TKey>` are used instead.

`GroupJoin` and `Join` require additional `Func<TResult, TKey> resultKeySelector` parameter.
`SelectMany` requires additional `OrderedEnumerable<TResult, TResultKey> seed` parameter.
They're needed to construct `OrderedEnumerable<TResult, TKey>` result.

## Why OrderedEnumerable\<T, TKey\> class and not IOrderedEnumerable\<T\>
Algorithms require `IComparer<T>` that was used to order the enumerable. Sadly `System.Linq.OrderedEnumerable` is `internal` and `IOrderedEnumerable<T>` doesn't have `IComparer<T>`.
