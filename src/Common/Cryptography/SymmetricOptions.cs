namespace Numaka.Common.Cryptography
{
    public class SymmetricOptions : HmacOptions
    {
        public SymmetricOptions()
        {
            InitializationVectorSize = 128;
            BlockSize = 128;               
            KeySize = 256;  
        }

        /// <summary>
        /// Base 64 key
        /// </summary>
        public string Key { get; set; }

        public int BlockSize { get; set; }

        public int InitializationVectorSize { get; set; }

        /// <summary>
        /// Can be 128, 192 or 256
        /// </summary>
        public int KeySize { get; set; }
    }
}
