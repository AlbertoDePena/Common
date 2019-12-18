using System;

namespace Numaka.Common.Cryptography
{
    public class KeyContainer
    {
        public KeyContainer(string privateKey, string publicKey)
        {
            PrivateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
        }

        /// <summary>
        /// Base 64 key
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Base 64 key
        /// </summary>
        public string PublicKey { get; set; }
    }
}
