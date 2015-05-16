using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public abstract class UnaryOperationTests
    {
        [TestCaseSource("GetOperateTestCases")]
        public IEnumerable<int> OperateByEqual(IEnumerable<int> source)
        {
            return Operate(source, (i1, i2) => i1.Equals(i2));
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<int> OperateInfiniteByEquals(IEnumerable<int> source)
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

        protected abstract IEnumerable<T> Operate<T>(IEnumerable<T> source, Func<T, T, bool> equal);

        protected abstract IEnumerable<UnaryTestCase<int>> GetOperateCases();
        protected abstract IEnumerable<UnaryTestCase<int>> GetOperateInfiniteCases();
    }
}
