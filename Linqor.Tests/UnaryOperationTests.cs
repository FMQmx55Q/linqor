using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class UnaryOperationTests
    {
        [TestCaseSource(typeof(DistinctTests), nameof(DistinctTests.GetOperateCases))]
        [TestCaseSource(typeof(GroupByTests), nameof(GroupByTests.GetOperateCases))]
        public IEnumerable<T2> OperateByEqual<T1, T2>(IEnumerable<T1> source, Func<IEnumerable<T1>, IEnumerable<T2>> operate)
        {
            return operate(source);
        }

        [TestCaseSource(typeof(DistinctTests), nameof(DistinctTests.GetOperateInfiniteCases))]
        [TestCaseSource(typeof(GroupByTests), nameof(GroupByTests.GetOperateInfiniteCases))]
        [MaxTime(3000)]
        public IEnumerable<T2> OperateInfiniteByEquals<T1, T2>(IEnumerable<T1> source, Func<IEnumerable<T1>, IEnumerable<T2>> operate)
        {
            return operate(source).Skip(10).Take(5);
        }
    }
}
