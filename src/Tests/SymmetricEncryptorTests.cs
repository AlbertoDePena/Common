using Xunit;
using Numaka.Common.Cryptography;

namespace Numaka.Common.Tests
{
    public class SymmetricEncryptorTests
    {
        [Fact]
        public void Encrypt_And_Decrypt()
        {
            var hmacKey = KeyGenerator.GenerateHmacKey();
            var key = KeyGenerator.GenerateKey();

            var encryptor = new SymmetricEncryptor(new SymmetricOptions() { HmacKey = hmacKey, Key = key });

            const string expected = "Some test";

            var encrypted = encryptor.Encrypt(expected);

            var decryptor = new SymmetricEncryptor(new SymmetricOptions() { HmacKey = hmacKey, Key = key });

            var decrypted = decryptor.DecryptAsString(encrypted);

            Assert.Equal(expected, decrypted);
        }
    }
}
