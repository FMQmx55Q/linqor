using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public abstract class BinaryOperationTests<TOuter, TInner, TExpected>
    {
        [TestCaseSource("GetOperateTestCases")]
        public IEnumerable<TExpected> OperateByCompare(IEnumerable<TOuter> outer, IEnumerable<TInner> inner)
        {
            return Operate(outer, inner);
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<TExpected> OperateInfiniteByCompare(IEnumerable<TOuter> outer, IEnumerable<TInner> inner)
        {
            return Operate(outer, inner).Skip(10).Take(5).ToArray();
        }

        protected IEnumerable<ITestCaseData> GetOperateTestCases()
        {
            return GetOperateCases().ToTestCases();
        }

        protected IEnumerable<ITestCaseData> GetOperateInfiniteTestCases()
        {
            return GetOperateInfiniteCases().ToTestCases();
        }

        protected abstract IEnumerable<BinaryTestCase<TOuter, TInner, TExpected>> GetOperateCases();
        protected abstract IEnumerable<BinaryTestCase<TOuter, TInner, TExpected>> GetOperateInfiniteCases();

        protected abstract IEnumerable<TExpected> Operate(IEnumerable<TOuter> outer, IEnumerable<TInner> inner);
    }
}
