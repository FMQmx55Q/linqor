using System;

namespace Linqor.Tests
{
    public class TestEntity
    {
        public static Func<TestEntity, int> GetKey = e => e.Id;

        public static TestEntity Create(int id)
        {
            return new TestEntity { Id = id };
        }

        public static bool IsEqual(TestEntity e1, TestEntity e2)
        {
            return e1.Id == e2.Id;
        }

        public static int Compare(TestEntity e1, TestEntity e2)
        {
            return e1.Id.CompareTo(e2.Id);
        }

        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            return Id.Equals(((TestEntity)obj).Id);
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
