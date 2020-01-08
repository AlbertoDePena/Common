using Xunit;
using Numaka.Cryptography;

namespace Numaka.Cryptography.Tests
{
    public class SlowEqualsTests
    {
        [Fact]
        public void Same_Arrays_Should_Match()
        {
            var first = new byte[] { 1, 2 };
            var second = new byte[] { 1, 2 };

            var result = SlowEquals.AreEqual(first, second);

            Assert.True(result);
        }

        [Fact]
        public void Different_Arrays_Should_Not_Match()
        {
            var first = new byte[] { 2, 1 };
            var second = new byte[] { 1, 2 };

            var result = SlowEquals.AreEqual(first, second);

            Assert.False(result);
        }
    }
}
