using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public abstract class BinaryOperationTests<TLeft, TRight, TExpected>
    {
        [TestCaseSource("GetOperateTestCases")]
        public IEnumerable<TExpected> OperateByCompare(IEnumerable<TLeft> left, IEnumerable<TRight> right)
        {
            return Operate(left, right).ToArray();
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<TExpected> OperateInfiniteByCompare(IEnumerable<TLeft> left, IEnumerable<TRight> right)
        {
            return Operate(left, right).Skip(10).Take(5).ToArray();
        }

        protected IEnumerable<ITestCaseData> GetOperateTestCases()
        {
            return GetOperateCases().ToTestCases();
        }

        protected IEnumerable<ITestCaseData> GetOperateInfiniteTestCases()
        {
            return GetOperateInfiniteCases().ToTestCases();
        }

        protected abstract IEnumerable<BinaryTestCase<TLeft, TRight, TExpected>> GetOperateCases();
        protected abstract IEnumerable<BinaryTestCase<TLeft, TRight, TExpected>> GetOperateInfiniteCases();

        protected abstract IEnumerable<TExpected> Operate(IEnumerable<TLeft> left, IEnumerable<TRight> right);
    }
}
