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
        public IEnumerable<int> OperateTest(IEnumerable<int> source)
        {
            return Operate(source);
        }

        [TestCaseSource("GetOperateByPredicateTestCases")]
        public IEnumerable<TestEntity> OperateByKey(IEnumerable<TestEntity> source)
        {
            return OperateByKey(source, TestEntity.GetKey);
        }

        [TestCaseSource("GetOperateByPredicateTestCases")]
        public IEnumerable<TestEntity> OperateByEqual(IEnumerable<TestEntity> source)
        {
            return OperateByEquals(source, TestEntity.IsEqual);
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<int> OperateInfinite(IEnumerable<int> source)
        {
            return Operate(source).Skip(10).Take(5);
        }

        [TestCaseSource("GetOperateInfiniteByPredicateTestCases")]
        [Timeout(3000)]
        public IEnumerable<TestEntity> OperateInfiniteByKey(IEnumerable<TestEntity> source)
        {
            return OperateByKey(source, TestEntity.GetKey).Skip(10).Take(5);
        }

        [TestCaseSource("GetOperateInfiniteByPredicateTestCases")]
        [Timeout(3000)]
        public IEnumerable<TestEntity> OperateInfiniteByEquals(IEnumerable<TestEntity> source)
        {
            return OperateByEquals(source, TestEntity.IsEqual).Skip(10).Take(5);
        }

        protected IEnumerable<ITestCaseData> GetOperateTestCases()
        {
            return GetOperateCases().ToTestCases();
        }

        protected IEnumerable<ITestCaseData> GetOperateByPredicateTestCases()
        {
            return GetOperateCases().ToEntityCases().ToTestCases();
        }

        protected IEnumerable<ITestCaseData> GetOperateInfiniteTestCases()
        {
            return GetOperateInfiniteCases().ToTestCases();
        }

        protected IEnumerable<ITestCaseData> GetOperateInfiniteByPredicateTestCases()
        {
            return GetOperateInfiniteCases().ToEntityCases().ToTestCases();
        }

        protected abstract IEnumerable<T> Operate<T>(IEnumerable<T> source)
            where T : IEquatable<T>;

        protected abstract IEnumerable<T> OperateByKey<T, TKey>(IEnumerable<T> source, Func<T, TKey> keySelector)
            where TKey : IEquatable<TKey>;

        protected abstract IEnumerable<T> OperateByEquals<T>(IEnumerable<T> source, Func<T, T, bool> equal);

        protected abstract IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>>> GetOperateCases();
        protected abstract IEnumerable<Tuple<string, IEnumerable<int>, IEnumerable<int>>> GetOperateInfiniteCases();
    }
}
