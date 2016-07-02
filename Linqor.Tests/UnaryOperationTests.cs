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
        public IEnumerable<TExpected> OperateByEqual(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TExpected>> operate)
        {
            return operate(source);
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<TExpected> OperateInfiniteByEquals(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TExpected>> operate)
        {
            return operate(source).Skip(10).Take(5);
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

        protected abstract IReadOnlyList<UnaryTestCase<TSource, TExpected>> GetOperateCases();
        protected abstract IReadOnlyList<UnaryTestCase<TSource, TExpected>> GetOperateInfiniteCases();

        protected abstract IReadOnlyList<Func<IEnumerable<TSource>, IEnumerable<TExpected>>> GetOperations();
    }
}
