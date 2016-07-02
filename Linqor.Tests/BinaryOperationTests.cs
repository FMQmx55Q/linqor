using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public abstract class BinaryOperationTests<TLeft, TRight, TExpected>
    {
        [TestCaseSource("GetOperateTestCases")]
        public IEnumerable<TExpected> OperateByCompare(IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<IEnumerable<TLeft>, IEnumerable<TRight>, IEnumerable<TExpected>> operate)
        {
            return operate(left, right).ToArray();
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<TExpected> OperateInfiniteByCompare(IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<IEnumerable<TLeft>, IEnumerable<TRight>, IEnumerable<TExpected>> operate)
        {
            return operate(left, right).Skip(10).Take(5).ToArray();
        }

        protected IEnumerable<ITestCaseData> GetOperateTestCases()
        {
            return GetOperations()
                .SelectMany(GetOperateCases().ToTestCases);
        }

        protected IEnumerable<ITestCaseData> GetOperateInfiniteTestCases()
        {
            return GetOperations()
                .SelectMany(GetOperateInfiniteCases().ToTestCases);
        }

        protected abstract IReadOnlyList<BinaryTestCase<TLeft, TRight, TExpected>> GetOperateCases();
        protected abstract IReadOnlyList<BinaryTestCase<TLeft, TRight, TExpected>> GetOperateInfiniteCases();

        protected abstract IReadOnlyList<Func<IEnumerable<TLeft>, IEnumerable<TRight>, IEnumerable<TExpected>>> GetOperations();
    }
}
