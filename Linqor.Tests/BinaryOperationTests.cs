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
        public IEnumerable<int> OperateTest(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return Operate(outer, inner);
        }

        [TestCaseSource("GetOperateByPredicateTestCases")]
        public IEnumerable<TestEntity> OperateByKey(IEnumerable<TestEntity> outer, IEnumerable<TestEntity> inner)
        {
            return OperateByKey(outer, inner, TestEntity.GetKey);
        }

        [TestCaseSource("GetOperateByPredicateTestCases")]
        public IEnumerable<TestEntity> OperateByCompare(IEnumerable<TestEntity> outer, IEnumerable<TestEntity> inner)
        {
            return OperateByCompare(outer, inner, TestEntity.Compare);
        }

        [TestCaseSource("GetOperateInfiniteTestCases")]
        [Timeout(3000)]
        public IEnumerable<int> OperateInfinite(IEnumerable<int> outer, IEnumerable<int> inner)
        {
            return Operate(outer, inner).Skip(10).Take(5);
        }

        [TestCaseSource("GetOperateInfiniteByPredicateTestCases")]
        [Timeout(3000)]
        public IEnumerable<TestEntity> OperateInfiniteByKey(IEnumerable<TestEntity> outer, IEnumerable<TestEntity> inner)
        {
            return OperateByKey(outer, inner, TestEntity.GetKey).Skip(10).Take(5);
        }

        [TestCaseSource("GetOperateInfiniteByPredicateTestCases")]
        [Timeout(3000)]
        public IEnumerable<TestEntity> OperateInfiniteByCompare(IEnumerable<TestEntity> outer, IEnumerable<TestEntity> inner)
        {
            return OperateByCompare(outer, inner, TestEntity.Compare).Skip(10).Take(5);
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

        protected abstract IEnumerable<BinaryTestCase<int>> GetOperateCases();
        protected abstract IEnumerable<BinaryTestCase<int>> GetOperateInfiniteCases();

        protected abstract IEnumerable<T> Operate<T>(IEnumerable<T> outer, IEnumerable<T> inner)
            where T : IComparable<T>;

        protected abstract IEnumerable<T> OperateByKey<T, TKey>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>;

        protected abstract IEnumerable<T> OperateByCompare<T>(IEnumerable<T> outer, IEnumerable<T> inner, Func<T, T, int> compare);
    }
}
