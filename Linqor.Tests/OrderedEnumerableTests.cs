using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class OrderedEnumerableTests
    {
        [Test]
        public void UnorderedDetection()
        {
            Assert.That(
                () => new[] { 3, 5, 1, 4, 2 }
                    .AsOrderedBy(x => x)
                    .ToArray(),
                Throws.TypeOf<UnorderedElementDetectedException>());
        }
    }
}