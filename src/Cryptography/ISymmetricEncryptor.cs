using System.IO;

namespace Numaka.Cryptography
{
    public interface ISymmetricEncryptor
    {
        byte[] Decrypt(byte[] bytes);

        byte[] Decrypt(Stream stream);

        byte[] Decrypt(string value);

        string DecryptAsString(byte[] bytes);

        string DecryptAsString(Stream stream);

        string DecryptAsString(string value);

        byte[] Encrypt(byte[] bytes);

        byte[] Encrypt(Stream stream);

        byte[] Encrypt(string value);

        string EncryptAsString(byte[] bytes);

        string EncryptAsString(Stream stream);

        string EncryptAsString(string value);
    }
}
