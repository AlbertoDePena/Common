namespace Numaka.Common.Cryptography
{
    public interface IAsymmetricEncryptor
    {
        byte[] Decrypt(byte[] bytes);

        byte[] Decrypt(string value);

        string DecryptAsString(byte[] bytes);

        string DecryptAsString(string value);

        byte[] Encrypt(byte[] bytes);

        byte[] Encrypt(string value);

        string EncryptAsString(byte[] bytes);

        string EncryptAsString(string value);
    }
}
