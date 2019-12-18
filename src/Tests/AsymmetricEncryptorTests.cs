using Xunit;
using Numaka.Common.Cryptography;

namespace Numaka.Common.Tests
{
    public class AsymmetricEncryptorTests
    {
        [Fact]
        public void Encrypt_And_Decrypt()
        {
            var hmacKey = KeyGenerator.GenerateHmacKey();
            var keys = KeyGenerator.GenerateKeys();

            var encryptor = new AsymmetricEncryptor(new AsymmetricOptions() { HmacKey = hmacKey, Key = keys.PublicKey });

            const string expected = "Some test";

            var encrypted = encryptor.Encrypt(expected);

            var decryptor = new AsymmetricEncryptor(new AsymmetricOptions() { HmacKey = hmacKey, Key = keys.PrivateKey });

            var decrypted = decryptor.DecryptAsString(encrypted);

            Assert.Equal(expected, decrypted);
        }
    }
}
