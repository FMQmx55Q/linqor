using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public abstract class UnaryOperationTests<TSource, TExpected>
    {
        [TestCaseSource("GetOperateTestCases")]
        public IEnumerable<TExpected> OperateByEqual(IEnumerable<TSource> source)
        {
            return Operate(source, (i1, i2) => i1.Equals(i2));
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<TExpected> OperateInfiniteByEquals(IEnumerable<TSource> source)
        {
            return Operate(source, (i1, i2) => i1.Equals(i2)).Skip(10).Take(5);
        }

        protected IEnumerable<ITestCaseData> GetOperateTestCases()
        {
            return GetOperateCases().ToTestCases();
        }

        protected IEnumerable<ITestCaseData> GetOperateInfiniteTestCases()
        {
            return GetOperateInfiniteCases().ToTestCases();
        }

        protected abstract IEnumerable<TExpected> Operate(IEnumerable<TSource> source, Func<TSource, TSource, bool> equal);

        protected abstract IEnumerable<UnaryTestCase<TSource, TExpected>> GetOperateCases();
        protected abstract IEnumerable<UnaryTestCase<TSource, TExpected>> GetOperateInfiniteCases();
    }
}
