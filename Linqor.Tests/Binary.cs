using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class Binary
    {
        [TestCaseSource(typeof(ConcatTests), nameof(ConcatTests.GetTestCases))]
        [TestCaseSource(typeof(ExceptTests), nameof(ExceptTests.GetOperateCases))]
        [TestCaseSource(typeof(GroupJoinTests), nameof(GroupJoinTests.GetOperateCases))]
        [TestCaseSource(typeof(IntersectTests), nameof(IntersectTests.GetOperateCases))]
        [TestCaseSource(typeof(JoinTests), nameof(JoinTests.GetOperateCases))]
        [TestCaseSource(typeof(UnionTests), nameof(UnionTests.GetOperateCases))]
        public IEnumerable<T3> Test<T1, T2, T3>(IEnumerable<T1> left, IEnumerable<T2> right, Func<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> operate)
        {
            return operate(left, right).ToArray();
        }

        [TestCaseSource(typeof(ConcatTests), nameof(ConcatTests.GetInfiniteTestCases))]
        [TestCaseSource(typeof(ExceptTests), nameof(ExceptTests.GetOperateInfiniteCases))]
        [TestCaseSource(typeof(GroupJoinTests), nameof(GroupJoinTests.GetOperateInfiniteCases))]
        [TestCaseSource(typeof(IntersectTests), nameof(IntersectTests.GetOperateInfiniteCases))]
        [TestCaseSource(typeof(UnionTests), nameof(UnionTests.GetOperateInfiniteCases))]
        [MaxTime(3000)]
        public IEnumerable<T3> TestInfinite<T1, T2, T3>(IEnumerable<T1> left, IEnumerable<T2> right, Func<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> operate)
        {
            return operate(left, right).Skip(10).Take(5).ToArray();
        }
    }
}
