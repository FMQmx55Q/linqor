# Linqor
[![Build Status](https://travis-ci.org/dangerozov/linqor.svg?branch=master)](https://travis-ci.org/dangerozov/linqor)

Linq extensions for ordered sequences. Provides better perfomance in comparison to standard extensions. Lazily handles large or even infinite sequences.

Provides **Concat**, **Distinct**, **Except**, **GroupBy**, **GroupJoin**, **Intersect**, **Join** and **Union**.

## Comparing to System.Linq
**Except**, **Intersect** and **Union** is not Set operations in Linqor. In order to make them behave this way use **Distinct** before or after operation.

| System.Linq |        Linqor        |
|-------------|----------------------|
| Concat      | Concat               |
| Distinct    | Distinct             |
| Except      | Except + Distinct    |
| GroupBy     | GroupBy              |
| GroupJoin   | GroupJoin            |
| Intersect   | Intersect + Distinct |
| Join        | Join                 |
| Union       | Union + Distinct     |

## Usage
With IEquatable or IComparable types
```csharp
new[] { 1, 1, 2, 2, 2 }.AsOrderedBy().Distinct(); // [ 1, 2 ]
new[] { 1, 3, 5 }.AsOrderedBy().Concat(new[] { 2, 4, 6 }.AsOrderedBy()); // [ 1, 2, 3, 4, 5, 6 ]
```
With sequences in descending order
```csharp
new[] { 3, 2, 1 }.AsOrderedBy().Intersect(new[] { 4, 3, 2 }, (left, right) => left.CompareTo(right) * -1); // [ 3, 2 ]
```
With multiple lazy sources of data
```csharp
CustomersFromDatabase.AsOrderedBy(c => c.Id).Union(CustomersFromFile.AsOrderedBy(c => c.Id)); // customers that don't exist in database will be read from file
```
With custom comparer
```csharp
Customers.AsOrderedBy().Concat(Customers.AsOrderedBy(), (left, right) => left.Id < right.Id ? -1 : left.Id == right.Id ? 0 : 1);
```
