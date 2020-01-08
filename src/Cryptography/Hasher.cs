using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Numaka.Cryptography
{
    public static class Hasher
	{
		public static byte[] CalculateHash(byte[] bytes)
		{
			byte[] hash = null;

			if(bytes != null)
			{
				using(var hashClient = new SHA512CryptoServiceProvider())
				{
					hash = hashClient.ComputeHash(bytes);
				}
			}

			return hash;
		}

		public static byte[] CalculateHash(Stream stream)
		{
			byte[] hash = null;

			if(stream != null)
			{
				using(var hashClient = new SHA512CryptoServiceProvider())
				{
					hash = hashClient.ComputeHash(stream);
				}
			}

			return hash;
		}

		public static byte[] CalculateHash(string value)
		{
			byte[] hash = null;

			if(!string.IsNullOrWhiteSpace(value))
			{
				var byteData = Encoding.UTF8.GetBytes(value);

				hash = CalculateHash(byteData);
			}

			return hash;
		}

		public static long CalculateHashNumber(byte[] bytes)
		{
			long hashNumber = -1;

			if(bytes != null)
			{
				var hashBytes = CalculateHash(bytes);

				if(hashBytes != null)
				{
					hashNumber = ConvertHashToNumber(Convert.ToBase64String(hashBytes));
				}
			}

			return hashNumber;
		}

		public static long CalculateHashNumber(Stream stream)
		{
			long hashNumber = -1;

			if(stream != null)
			{
				var hashBytes = CalculateHash(stream);

				if(hashBytes != null)
				{
					hashNumber = ConvertHashToNumber(Convert.ToBase64String(hashBytes));
				}
			}

			return hashNumber;
		}

		public static long CalculateHashNumber(string value)
		{
			long hashNumber = -1;

            if (!string.IsNullOrWhiteSpace(value))
            {
				var hashBytes = CalculateHash(value);

				if(hashBytes != null)
				{
					hashNumber = ConvertHashToNumber(Convert.ToBase64String(hashBytes));
				}
			}

			return hashNumber;
		}

		public static string CalculateHashAsString(byte[] bytes)
		{
			string hashString = null;

			if(bytes != null)
			{
				var hashBytes = CalculateHash(bytes);

				if(hashBytes != null)
				{
					hashString = Convert.ToBase64String(hashBytes);
				}
			}

			return hashString;
		}

		public static string CalculateHashAsString(Stream stream)
		{
			string hashString = null;

			if(stream != null)
			{
				var hashBytes = CalculateHash(stream);

				if(hashBytes != null)
				{
					hashString = Convert.ToBase64String(hashBytes);
				}
			}

			return hashString;
		}

		public static string CalculateHashAsString(string value)
		{
			string hashString = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
				var hashBytes = CalculateHash(value);

				if(hashBytes != null)
				{
					hashString = Convert.ToBase64String(hashBytes);
				}
			}

			return hashString;
		}

		public static long ConvertHashToNumber(string hash)
		{
			long number = -1;

			if(!string.IsNullOrWhiteSpace(hash))
			{
				var hashBytes = Convert.FromBase64String(hash);

				number = BitConverter.ToInt64(hashBytes, 0);
			}

			return number;
		}
	}
}