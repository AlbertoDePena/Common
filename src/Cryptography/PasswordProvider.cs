using System;
using System.Security.Cryptography;
using System.Text;

namespace Numaka.Cryptography
{
    public static class PasswordProvider
    {
        private const int AlgorithmIndex = 0;
        private const int Bytes = 64;
        private const int IterationIndex = 1;
        private const int Pbkdf2Index = 4;

        private const int Pbkdf2Iterations = 64000;
        private const int SaltBytes = 64;
        private const int SaltIndex = 3;
        private const int Sections = 5;
        private const int SizeIndex = 2;

        public static string SecurePassword(string password)
        {
            var salt = new byte[SaltBytes];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            var hash = Pbkdf2(password, salt, Pbkdf2Iterations, Bytes);

            var builder = new StringBuilder();

            builder.Append("HMACSHA512:");
            builder.Append(Pbkdf2Iterations);
            builder.Append(":");
            builder.Append(hash.Length);
            builder.Append(":");
            builder.Append(Convert.ToBase64String(salt));
            builder.Append(":");
            builder.Append(Convert.ToBase64String(hash));

            return builder.ToString();
        }

        public static bool ValidatePassword(string password, string storedHash)
        {
            var isValid = false;

            var split = storedHash.Split(':');

            //make sre we have the correct number of parts
            if (split.Length == Sections)
            {
                //make sure the algorithm is correct
                if (split[AlgorithmIndex] == "HMACSHA512")
                {
                    if (int.TryParse(split[IterationIndex], out var iterations))
                    {
                        if (iterations > 0)
                        {
                            var salt = Convert.FromBase64String(split[SaltIndex]);
                            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

                            if (int.TryParse(split[SizeIndex], out var storedHashSize))
                            {
                                //make sure the hash is the right size
                                if (storedHashSize == hash.Length)
                                {
                                    var tmpHash = Pbkdf2(password, salt, iterations, hash.Length);

                                    isValid = SlowEquals.AreEqual(hash, tmpHash);
                                }
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var provider = new Pbkdf2Provider(password, salt, iterations))
                return provider.GetDerivedKeyBytes(outputBytes);
        }
    }
}
