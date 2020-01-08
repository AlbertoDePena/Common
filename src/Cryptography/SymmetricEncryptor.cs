using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Numaka.Cryptography
{
    public class SymmetricEncryptor : ISymmetricEncryptor
    {
        private readonly SymmetricOptions Options;

        public SymmetricEncryptor(SymmetricOptions options)
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
                    var iv = GetIv(validHmac);

                    if (iv != null)
                    {
                        var encrypted = GetEncrypted(bytes);

                        if (encrypted != null)
                        {
                            using (var provider = new RijndaelManaged())
                            {
                                provider.BlockSize = Options.BlockSize;
                                provider.KeySize = Options.KeySize;
                                provider.Padding = PaddingMode.PKCS7;
                                provider.Mode = CipherMode.CBC;

                                var key = Convert.FromBase64String(Options.Key);

                                if (key != null)
                                {
                                    using (var decryptor = provider.CreateDecryptor(key, iv))
                                    {
                                        using (var memoryStream = new MemoryStream())
                                        {
                                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                                            {
                                                cryptoStream.Write(encrypted, 0, encrypted.Length);
                                            }

                                            decrypted = memoryStream.ToArray();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return decrypted;
        }

        public byte[] Decrypt(Stream stream)
        {
            byte[] decrypted = null;

            if (stream != null)
            {
                byte[] encryptedData = null;

                using (var memoryStream = new MemoryStream())
                {
                    var buffer = new byte[1024];
                    var read = stream.Read(buffer, 0, buffer.Length);

                    while (read > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                        read = stream.Read(buffer, 0, buffer.Length);
                    }

                    encryptedData = memoryStream.ToArray();
                }

                decrypted = Decrypt(encryptedData);
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

        public string DecryptAsString(Stream stream)
        {
            string decryptedString = null;

            if (stream != null)
            {
                var decrypted = Decrypt(stream);

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
                using (var provider = new RijndaelManaged())
                {
                    provider.BlockSize = Options.BlockSize;
                    provider.KeySize = Options.KeySize;
                    provider.Padding = PaddingMode.PKCS7;
                    provider.Mode = CipherMode.CBC;

                    var key = Convert.FromBase64String(Options.Key);
                    var iv = GenerateIv(Options.InitializationVectorSize);

                    if (key != null && iv != null)
                    {
                        using (var encryptor = provider.CreateEncryptor(key, iv))
                        {
                            byte[] encryptedData = null;

                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(bytes, 0, bytes.Length);
                                }

                                encryptedData = memoryStream.ToArray();
                            }

                            encrypted = HmacEncryptedData(encryptedData, iv);
                        }
                    }
                }
            }

            return encrypted;
        }

        public byte[] Encrypt(Stream stream)
        {
            byte[] encrypted = null;

            if (stream != null)
            {
                using (var provider = new RijndaelManaged())
                {
                    provider.BlockSize = Options.BlockSize;
                    provider.KeySize = Options.KeySize;
                    provider.Padding = PaddingMode.PKCS7;
                    provider.Mode = CipherMode.CBC;

                    var key = Convert.FromBase64String(Options.Key);
                    var iv = GenerateIv(Options.InitializationVectorSize);

                    if (key != null && iv != null)
                    {
                        var encryptor = provider.CreateEncryptor(key, iv);

                        byte[] encryptedData = null;

                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                var buffer = new byte[1024];
                                var read = stream.Read(buffer, 0, buffer.Length);

                                while (read > 0)
                                {
                                    cryptoStream.Write(buffer, 0, read);
                                    read = stream.Read(buffer, 0, buffer.Length);
                                }
                            }

                            encryptedData = memoryStream.ToArray();
                        }

                        encrypted = HmacEncryptedData(encryptedData, iv);
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

        public string EncryptAsString(Stream stream)
        {
            string encryptedString = null;

            if (stream != null)
            {
                var encrypted = Encrypt(stream);

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

        private byte[] GetEncrypted(byte[] encryptedData)
        {
            byte[] encrypted = null;

            if (encryptedData != null)
            {
                var hmacByteLength = Options.HmacSize / 8;
                var ivByteLength = Options.InitializationVectorSize / 8;
                var totalLength = hmacByteLength + ivByteLength;

                if (encryptedData.Length > ivByteLength)
                {
                    var startIndex = encryptedData.Length - totalLength;
                    var encryptedBytes = new byte[startIndex];

                    Buffer.BlockCopy(encryptedData, 0, encryptedBytes, 0, startIndex);

                    encrypted = encryptedBytes;
                }
            }

            return encrypted;
        }

        private byte[] GetIv(byte[] encryptedData)
        {
            byte[] iv = null;

            if (encryptedData != null)
            {
                var ivByteLength = Options.InitializationVectorSize / 8;

                if (encryptedData.Length > ivByteLength)
                {
                    var startIndex = encryptedData.Length - ivByteLength;
                    var ivBytes = new byte[ivByteLength];

                    Buffer.BlockCopy(encryptedData, startIndex, ivBytes, 0, ivByteLength);

                    iv = ivBytes;
                }
            }

            return iv;
        }

        private byte[] HmacEncryptedData(byte[] encryptedData, byte[] iv)
        {
            byte[] encryptedHmac = null;

            if (encryptedData != null && iv != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(encryptedData, 0, encryptedData.Length);
                    memoryStream.Write(iv, 0, iv.Length);

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

        private byte[] GenerateIv(int initializationVectorSize)
        {
            var iv = new byte[initializationVectorSize / 8];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(iv);
            }

            return iv;
        }
    }
}