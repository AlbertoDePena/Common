using System;
using System.Security.Cryptography;
using System.Text;

namespace Numaka.Cryptography
{
    public static class SignatureProvider
	{
		public static byte[] CreateSignature(string key, byte[] bytes)
		{
			byte[] signature = null;

			if(!string.IsNullOrWhiteSpace(key) && bytes != null)
			{
				using(var provider = new RSACryptoServiceProvider())
				{
					if(!string.IsNullOrWhiteSpace(key))
					{
						var base64String = Convert.FromBase64String(key);

						provider.ImportCspBlob(base64String);

						signature = provider.SignData(bytes, new SHA512CryptoServiceProvider());
					}
				}
			}

			return signature;
		}

		public static byte[] CreateSignature(string key, string value)
		{
			byte[] signature = null;

			if(!string.IsNullOrWhiteSpace(value))
			{
				var bytes = Encoding.UTF8.GetBytes(value);

				if(bytes != null)
				{
					signature = CreateSignature(key, bytes);
				}
			}

			return signature;
		}

		public static string CreateSignatureAsString(string key, byte[] bytes)
		{
			string signature = null;

			if(bytes != null)
			{
				var signed = CreateSignature(key, bytes);

				if(signed != null)
				{
					signature = Convert.ToBase64String(signed);
				}
			}

			return signature;
		}

		public static string CreateSignatureAsString(string key, string value)
		{
			string signature = null;

			if(!string.IsNullOrWhiteSpace(value))
			{
				var signed = CreateSignature(key, value);

				if(signed != null)
				{
					signature = Convert.ToBase64String(signed);
				}
			}

			return signature;
		}

		public static bool VerifySignature(string key, byte[] originalBytes, byte[] signedBytes)
		{
			var verified = false;

			if(!string.IsNullOrWhiteSpace(key) && originalBytes != null && signedBytes != null)
			{
				using(var provider = new RSACryptoServiceProvider())
				{
					if(!string.IsNullOrWhiteSpace(key))
					{
						var bytes = Convert.FromBase64String(key);

						provider.ImportCspBlob(bytes);

						verified = provider.VerifyData(originalBytes, new SHA512CryptoServiceProvider(), signedBytes);
					}
				}
			}

			return verified;
		}

		public static bool VerifySignature(string key, string originalValue, string signedValue)
		{
			var verified = false;

			if(!string.IsNullOrWhiteSpace(originalValue) && !string.IsNullOrWhiteSpace(signedValue))
			{
				var original = Encoding.UTF8.GetBytes(originalValue);
                var signed = Convert.FromBase64String(signedValue);

				if(original != null)
				{
					verified = VerifySignature(key, original, signed);
				}
			}

			return verified;
		}
	}
}