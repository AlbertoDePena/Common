namespace Numaka.Common.Cryptography
{
    public class AsymmetricOptions : HmacOptions
    {
        /// <summary>
        /// Base 64 key
        /// </summary>
        public string Key { get; set; }
    }
}
