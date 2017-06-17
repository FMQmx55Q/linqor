using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    public static class DistinctTests
    {
        public static IReadOnlyList<TestCaseData> GetOperateCases()
        {
            var create = UnaryTestCase.GetCreator<int, string>("Distinct");
            var testCases = new[]
            {
                create(new int[] { }, new string[] { }),
                create(new int[] { 0 }, new string[] { "S-0-0" }),
                create(new int[] { 0, 1, 2 }, new string[] { "S-0-0", "S-1-1", "S-2-2" }),
                create(new int[] { 0, 1, 2, 2, 3, 3, 3, 4 }, new string[] { "S-0-0", "S-1-1", "S-2-2", "S-4-3", "S-7-4" })
            };
            
            return GetOperations().SelectMany(testCases.ToTestCases).ToArray();
        }

        public static IReadOnlyList<TestCaseData> GetOperateInfiniteCases()
        {
            var create = UnaryTestCase.GetCreator<int, string>("Distinct ∞");
            var testCases = new[]
            {
                create(Extensions.Generate(0, 2, 1), new string[] { "S-20-10", "S-22-11", "S-24-12", "S-26-13", "S-28-14" }),
                create(Extensions.Generate(0, 3, 1), new string[] { "S-30-10", "S-33-11", "S-36-12", "S-39-13", "S-42-14" })
            };

            return GetOperations().SelectMany(testCases.ToTestCases).ToArray();
        }

        public static IReadOnlyList<Func<IEnumerable<int>, IEnumerable<string>>> GetOperations()
        {
            return new Func<IEnumerable<int>, IEnumerable<string>>[]
            {
                (source) => source.ToEntities("S").AsOrderedBy(s => s.Value)
                    .Distinct((l, r) => l.Equals(r))
                    .Select(e => e.Key),
                (source) => source.ToEntities("S").AsOrderedBy(s => s.Value)
                    .Distinct()
                    .Select(e => e.Key)
            };
        }
    }
}
