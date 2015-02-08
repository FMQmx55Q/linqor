using System.Linq;
using NUnit.Framework;

namespace Linqor.Tests
{
    [TestFixture]
    public class JoinTests
    {
        [Test]
        public void Should_OrderedJoin()
        {
            var outer = new[]
            {
                new { Id = 0, Name = "Zero" },
                new { Id = 1, Name = "One" },
                new { Id = 2, Name = "Two" },
                new { Id = 3, Name = "Three" }
            };

            var inner = new[]
            {
                new { Id = 1, Text = "First" },
                new { Id = 2, Text = "Second" },
                new { Id = 3, Text = "Third" },
                new { Id = 4, Text = "Fourth" }
            };

            var actual = outer.OrderedJoin(inner, o => o.Id, i => i.Id, (o, i) => new { o.Id, o.Name, i.Text }).ToArray();

            Assert.That(actual[0], Is.EqualTo(new { Id = 1, Name = "One", Text = "First" }));
            Assert.That(actual[1], Is.EqualTo(new { Id = 2, Name = "Two", Text = "Second" }));
            Assert.That(actual[2], Is.EqualTo(new { Id = 3, Name = "Three", Text = "Third" }));
        }

        [Test]
        [Timeout(5000)]
        public void Should_OrderedJoin_Infinite()
        {
            var outer = TestCases.Generate(0, 1, 1).Select(i => new { Id = i, Name = i.ToString() });
            var inner = TestCases.Generate(1, 1, 1).Select(i => new { Id = i, Text = i.ToString() });

            var actual = outer.OrderedJoin(inner, o => o.Id, i => i.Id, (o, i) => new { o.Id, o.Name, i.Text }).Take(3).ToArray();

            Assert.That(actual[0], Is.EqualTo(new { Id = 1, Name = "1", Text = "1" }));
            Assert.That(actual[1], Is.EqualTo(new { Id = 2, Name = "2", Text = "2" }));
            Assert.That(actual[2], Is.EqualTo(new { Id = 3, Name = "3", Text = "3" }));
        }
    }
}
