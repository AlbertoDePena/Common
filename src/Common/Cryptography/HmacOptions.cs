namespace Numaka.Common.Cryptography
{
    public class HmacOptions
	{
		public HmacOptions()
		{
			HmacSize = 512;
		}

        /// <summary>
        /// Base 64 key
        /// </summary>
		public string HmacKey { get; set; }

		public int HmacSize { get; set; }
	}
}