using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static Linqor.Tests.Helpers;

namespace Linqor.Tests
{
    public static class Helpers
    {
        public static int NumberInString(string x) =>
            int.Parse(x.Substring(1));

        public static string Get2DID(int index, int length) =>
            $"{index / length}:{index % length}";
    }

    public class NumberInStringComparer : IEqualityComparer<string>
    {
        public bool Equals([AllowNull] string x, [AllowNull] string y) =>
            NumberInString(x).Equals(NumberInString(y));

        public int GetHashCode([DisallowNull] string obj) =>
            NumberInString(obj).GetHashCode();
    }
}