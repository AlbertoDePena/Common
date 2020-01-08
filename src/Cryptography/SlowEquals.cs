namespace Numaka.Cryptography
{
    public static class SlowEquals
    {
        public static bool AreEqual(byte[] first, byte[] second)
        {
            var equal = first == null && second == null;

            if (first != null && second != null)
            {
                var dif = first.Length ^ second.Length;

                for (var i = 0; i < first.Length && i < second.Length; i++)
                {
                    dif |= first[i] ^ second[i];
                }

                if (dif == 0)
                {
                    equal = true;
                }
            }

            return equal;
        }
    }
}