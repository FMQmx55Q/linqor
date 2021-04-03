using System.Collections.Generic;

namespace Linqor.Tests
{
    public static class Helpers
    {
        public static int ID(this string s) => int.Parse(s.Substring(1));

        public static IEqualityComparer<string> ByID => new ECByID();

        private class ECByID : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => x.ID().Equals(y.ID());
            public int GetHashCode(string obj) => obj.ID().GetHashCode();
        }

        public static string Get2DID(int index, int length) =>
            $"{index / length}:{index % length}";
    }
}