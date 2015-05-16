using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public abstract class BinaryOperationTests
    {
        [TestCaseSource("GetOperateTestCases")]
        public IEnumerable<int> OperateByCompare(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return Operate(outer, inner, (i1, i2) => i1.CompareTo(i2));
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<int> OperateInfiniteByCompare(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return Operate(outer, inner, (i1, i2) => i1.CompareTo(i2)).Skip(10).Take(5);
        }

        protected IEnumerable<ITestCaseData> GetOperateTestCases()
        {
            return GetOperateCases().ToTestCases();
        }

        protected IEnumerable<ITestCaseData> GetOperateInfiniteTestCases()
        {
            return GetOperateInfiniteCases().ToTestCases();
        }

        protected abstract IEnumerable<BinaryTestCase<int>> GetOperateCases();
        protected abstract IEnumerable<BinaryTestCase<int>> GetOperateInfiniteCases();

        protected abstract IEnumerable<T> Operate<T>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare);
    }
}
