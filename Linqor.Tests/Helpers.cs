using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    public static class Helpers
    {
        public static int NumberInString(string x) =>
            int.Parse(x.Substring(1));

        public static string Get2DID(int index, int length) =>
            $"{index / length}:{index % length}";

        public static T[] Repeat<T>(T element, int count) =>
            Enumerable.Repeat(element, count).ToArray();
    }

    public class NumberInStringComparer : IEqualityComparer<string>
    {
        public bool Equals([AllowNull] string x, [AllowNull] string y) =>
            NumberInString(x).Equals(NumberInString(y));

        public int GetHashCode([DisallowNull] string obj) =>
            NumberInString(obj).GetHashCode();
    }

    public class IntComparer : IComparer<int>
    {
        public int Compare(int x, int y) => x.CompareTo(y);
    }

    public class StringComparer : IComparer<string>
    {
        public int Compare(string x, string y) => x.CompareTo(y);
    }
}