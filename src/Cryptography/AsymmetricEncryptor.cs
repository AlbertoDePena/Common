using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Numaka.Cryptography
{
    public class AsymmetricEncryptor : IAsymmetricEncryptor
    {
        private readonly AsymmetricOptions Options;

        public AsymmetricEncryptor(AsymmetricOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public byte[] Decrypt(byte[] bytes)
        {
            byte[] decrypted = null;

            if (bytes != null)
            {
                var validHmac = ValidateHmac(bytes);

                if (validHmac != null)
                {
                    using (var provider = new RSACryptoServiceProvider())
                    {
                        if (!string.IsNullOrWhiteSpace(Options.Key))
                        {
                            var key = Convert.FromBase64String(Options.Key);

                            provider.ImportCspBlob(key);

                            decrypted = provider.Decrypt(validHmac, true);
                        }
                    }
                }
            }

            return decrypted;
        }

        public byte[] Decrypt(string value)
        {
            byte[] decrypted = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                var byteData = Convert.FromBase64String(value);

                decrypted = Decrypt(byteData);
            }

            return decrypted;
        }

        public string DecryptAsString(byte[] bytes)
        {
            string decryptedString = null;

            if (bytes != null)
            {
                var decrypted = Decrypt(bytes);

                if (decrypted != null)
                {
                    decryptedString = Encoding.UTF8.GetString(decrypted);
                }
            }

            return decryptedString;
        }

        public string DecryptAsString(string value)
        {
            string decryptedString = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                var decrypted = Decrypt(value);

                if (decrypted != null)
                {
                    decryptedString = Encoding.UTF8.GetString(decrypted);
                }
            }

            return decryptedString;
        }

        public byte[] Encrypt(byte[] bytes)
        {
            byte[] encrypted = null;

            if (bytes != null)
            {
                using (var provider = new RSACryptoServiceProvider())
                {
                    if (!string.IsNullOrWhiteSpace(Options.Key))
                    {
                        var key = Convert.FromBase64String(Options.Key);

                        provider.ImportCspBlob(key);

                        var encryptedData = provider.Encrypt(bytes, true);

                        if (encryptedData != null)
                        {
                            encrypted = HmacEncryptedData(encryptedData);
                        }
                    }
                }
            }

            return encrypted;
        }

        public byte[] Encrypt(string value)
        {
            byte[] encrypted = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                var byteData = Encoding.UTF8.GetBytes(value);

                if (byteData != null)
                {
                    encrypted = Encrypt(byteData);
                }
            }

            return encrypted;
        }

        public string EncryptAsString(byte[] bytes)
        {
            string encryptedString = null;

            if (bytes != null)
            {
                var encrypted = Encrypt(bytes);

                if (encrypted != null)
                {
                    encryptedString = Convert.ToBase64String(encrypted);
                }
            }

            return encryptedString;
        }

        public string EncryptAsString(string value)
        {
            string encryptedString = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                var encrypted = Encrypt(value);

                if (encrypted != null)
                {
                    encryptedString = Convert.ToBase64String(encrypted);
                }
            }

            return encryptedString;
        }

        private byte[] HmacEncryptedData(byte[] encryptedData)
        {
            byte[] encryptedHmac = null;

            if (encryptedData != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(encryptedData, 0, encryptedData.Length);

                    var hmac = HmacProvider.CalculateHash(Options.HmacKey, memoryStream.ToArray());

                    if (hmac != null)
                    {
                        memoryStream.Write(hmac, 0, hmac.Length);
                        encryptedHmac = memoryStream.ToArray();
                    }
                }
            }

            return encryptedHmac;
        }

        private byte[] ValidateHmac(byte[] encryptedData)
        {
            byte[] validatedHmac = null;

            if (encryptedData != null)
            {
                var hmacByteLength = Options.HmacSize / 8;

                if (encryptedData.Length > hmacByteLength)
                {
                    var startIndex = encryptedData.Length - hmacByteLength;
                    var dataLength = encryptedData.Length - hmacByteLength;
                    var hmac = new byte[hmacByteLength];
                    var hmacData = new byte[dataLength];

                    Buffer.BlockCopy(encryptedData, 0, hmacData, 0, dataLength);
                    Buffer.BlockCopy(encryptedData, startIndex, hmac, 0, hmacByteLength);

                    var isValid = HmacProvider.ValidateHmac(Options.HmacKey, hmacData, hmac);

                    if (isValid)
                    {
                        validatedHmac = hmacData;
                    }
                }
            }

            return validatedHmac;
        }
    }
}