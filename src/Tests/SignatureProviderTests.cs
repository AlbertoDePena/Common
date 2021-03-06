﻿using Xunit;
using Numaka.Cryptography;

namespace Numaka.Cryptography.Tests
{
    public class SignatureProviderTests
    {
        [Fact]
        public void Sign_And_Verify()
        {
            const string expected = "Some text";
            var key = KeyGenerator.GenerateKeys().PrivateKey;

            var signature = SignatureProvider.CreateSignatureAsString(key, expected);
            var isValid = SignatureProvider.VerifySignature(key, expected, signature);

            Assert.True(isValid);
        }
    }
}
