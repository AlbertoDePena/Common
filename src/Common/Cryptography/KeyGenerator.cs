using System;
using System.Security.Cryptography;

namespace Numaka.Common.Cryptography
{
    public static class KeyGenerator
    {
        public static KeyContainer GenerateKeys()
        {
            using (var provider = new RSACryptoServiceProvider(4096))
            {
                var privateKeyBytes = provider.ExportCspBlob(includePrivateParameters: true);
                var publicKeyBytes = provider.ExportCspBlob(includePrivateParameters: false);

                var privateKey = Convert.ToBase64String(privateKeyBytes);
                var publicKey = Convert.ToBase64String(publicKeyBytes);

                return new KeyContainer(privateKey, publicKey);
            }
        }

        public static string GenerateKey()
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var bytes = new byte[32];

                random.GetBytes(bytes);

                return Convert.ToBase64String(bytes);
            }
        }

        public static string GenerateHmacKey()
        {
            using (var hmac = new HMACSHA512())
            {
                return Convert.ToBase64String(hmac.Key);
            }
        }
    }
}
