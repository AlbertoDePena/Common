using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Numaka.Cryptography
{
    public static class HmacProvider
    {
        public static byte[] CalculateHash(string hmacKey, byte[] bytes)
        {
            byte[] hash = null;

            if (!string.IsNullOrWhiteSpace(hmacKey) && bytes != null)
            {
                var key = Convert.FromBase64String(hmacKey);

                if (key != null)
                {
                    using (var hmacClient = new HMACSHA512(key))
                    {
                        hash = hmacClient.ComputeHash(bytes);
                    }
                }
            }

            return hash;
        }

        public static byte[] CalculateHash(string hmacKey, Stream stream)
        {
            byte[] hash = null;

            if (!string.IsNullOrWhiteSpace(hmacKey) && stream != null)
            {
                var key = Convert.FromBase64String(hmacKey);

                if (key != null)
                {
                    using (var hmacClient = new HMACSHA512(key))
                    {
                        hash = hmacClient.ComputeHash(stream);
                    }
                }
            }

            return hash;
        }

        public static byte[] CalculateHash(string hmacKey, string value)
        {
            byte[] hash = null;

            if (!string.IsNullOrWhiteSpace(hmacKey) && !string.IsNullOrWhiteSpace(value))
            {
                var key = Convert.FromBase64String(hmacKey);
                var toHmac = Encoding.UTF8.GetBytes(value);

                if (key != null && toHmac != null)
                {
                    using (var hmacClient = new HMACSHA512(key))
                    {
                        hash = hmacClient.ComputeHash(toHmac);
                    }
                }
            }

            return hash;
        }

        public static string CalculateHashAsString(string hmacKey, byte[] bytes)
        {
            string hashString = null;

            if (bytes != null)
            {
                var hmac = CalculateHash(hmacKey, bytes);

                if (hmac != null)
                {
                    hashString = Convert.ToBase64String(hmac);
                }
            }

            return hashString;
        }

        public static string CalculateHashAsString(string hmacKey, Stream stream)
        {
            string hashString = null;

            if (stream != null)
            {
                var hmac = CalculateHash(hmacKey, stream);

                if (hmac != null)
                {
                    hashString = Convert.ToBase64String(hmac);
                }
            }

            return hashString;
        }

        public static string CalculateHashAsString(string hmacKey, string value)
        {
            string hashString = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                var hmac = CalculateHash(hmacKey, value);

                if (hmac != null)
                {
                    hashString = Convert.ToBase64String(hmac);
                }
            }

            return hashString;
        }

        public static bool ValidateHmac(string hmacKey, byte[] bytes, byte[] hmac)
        {
            var validated = false;

            if (bytes != null && hmac != null)
            {
                var calculated = CalculateHash(hmacKey, bytes);

                if (calculated != null)
                {
                    validated = SlowEquals.AreEqual(hmac, calculated);
                }
            }

            return validated;
        }
    }
}