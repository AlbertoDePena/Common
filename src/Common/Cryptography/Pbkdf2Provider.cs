using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Numaka.Common.Cryptography
{
    public class Pbkdf2Provider : IDisposable
    {
        private const int CMinIterations = 1000;
        private const int CMinSaltLength = 8;

        private readonly HMACSHA512 _hasher;
        private readonly int _hashLength;
        private readonly byte[] _password;
        private readonly byte[] _salt;
        private readonly int _iterations;
        private bool _isDisposing;

        /// <summary>
        ///     Rfc2898 constructor to create Rfc2898 object ready to perform Rfc2898 functionality
        /// </summary>
        /// <param name="password">The Password to be hashed and is also the HMAC key</param>
        /// <param name="salt">Salt to be concatenated with the password</param>
        /// <param name="iterations">Number of iterations to perform HMAC Hashing for PBKDF2</param>
        public Pbkdf2Provider(byte[] password, byte[] salt, int iterations)
        {
            if (iterations < CMinIterations) throw new InvalidOperationException("Iteration count is less than the 1000 recommended in Rfc2898");

            if (salt?.Length < CMinSaltLength) throw new InvalidOperationException("Salt is less than the 8 byte size recommended in Rfc2898");

            _hasher = new HMACSHA512(password);
            _hashLength = _hasher.HashSize / 8;
            _password = password;
            _salt = salt;
            _iterations = iterations;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Pbkdf2Provider" /> class.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The iterations.</param>
        /// <inheritdoc />
        public Pbkdf2Provider(string password, byte[] salt, int iterations)
            : this(new UTF8Encoding(false).GetBytes(password), salt, iterations) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Pbkdf2Provider" /> class.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The iterations.</param>
        /// <inheritdoc />
        public Pbkdf2Provider(string password, string salt, int iterations)
            : this(new UTF8Encoding(false).GetBytes(password), new UTF8Encoding(false).GetBytes(salt), iterations) { }

        /// <summary>
        ///     Derive Key Bytes using PBKDF2 specification listed in Rfc2898 and HMAC as the underlying PRF (Pseudo Random
        ///     Function)
        /// </summary>
        /// <param name="keyLength">Length in Bytes of Derived Key</param>
        /// <returns>Derived Key</returns>
        public byte[] GetDerivedKeyBytes(int keyLength)
        {         
            var l = Math.Ceiling((double)keyLength / _hashLength);

            var finalBlock = Array.Empty<byte>();

            for (var i = 1; i <= l; i++)
            {
                //Concatenate each block from F into the final block (T_1..T_l)
                finalBlock = MergeByteArrays(finalBlock, F(_password, _salt, _iterations, i));
            }

            //returning DK note r not used as dkLen bytes of the final concatenated block returned rather than <0...r-1> substring of final intermediate block + prior blocks as per spec
            return finalBlock.Take(keyLength).ToArray();
        }

        /// <summary>
        ///     A publicly exposed version of GetDerivedKeyBytes_PBKDF2_HMAC which matches the exact specification in Rfc2898
        ///     PBKDF2 using HMAC
        /// </summary>
        /// <param name="password">Password passed as a Byte Array</param>
        /// <param name="salt">Salt passed as a Byte Array</param>
        /// <param name="iterations">Iterations to perform the underlying PRF over</param>
        /// <param name="keyLength">Length of Bytes to return, an AES 256 key word require 32 Bytes</param>
        /// <returns>Derived Key in Byte Array form ready for use by chosen encryption function</returns>
        public byte[] Pbkdf2(byte[] password, byte[] salt, int iterations, int keyLength)
        {
            using (var provider = new Pbkdf2Provider(password, salt, iterations))
                return provider.GetDerivedKeyBytes(keyLength);
        }

        /// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <inheritdoc />
		public void Dispose()
        {
            if (!_isDisposing)
            {
                _isDisposing = true;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            else
            {
                Dispose(false);
            }
        }

        /// <summary>
		///     Implements IDisposable
		/// </summary>
		/// <param name="disposing">
		///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
		///     unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hasher.Dispose();
            }
        }

        private byte[] F(byte[] password, byte[] salt, int iterations, int i)
        {
            //Salt and Block number Int(i) concatenated as per spec
            var si = MergeByteArrays(salt, Int(i));

            //Initial hash (U_1) using password and salt concatenated with Int(i) as per spec
            var temp = Prf(password, si);

            //Output block filled with initial hash value or U_1 as per spec
            var uC = temp;

            for (var index = 1; index < iterations; index++)
            {
                //rehashing the password using the previous hash value as salt as per spec
                temp = Prf(password, temp);

                for (var j = 0; j < temp.Length; j++)
                {
                    //xor each byte of the each hash block with each byte of the output block as per spec
                    uC[j] ^= temp[j];
                }
            }

            //return a T_i block for concatenation to create the final block as per spec
            return uC;
        }

        private byte[] Int(int value)
        {
            var bytes = BitConverter.GetBytes(value);

            //Make sure most significant bit is first
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        private byte[] MergeByteArrays(byte[] first, byte[] second)
        {
            var buffer = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, buffer, 0, first.Length);
            Buffer.BlockCopy(second, 0, buffer, first.Length, second.Length);

            return buffer;
        }

        private byte[] Prf(byte[] first, byte[] second) => _hasher.ComputeHash(MergeByteArrays(first, second));
    }
}
