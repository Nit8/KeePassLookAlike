namespace KeePassLookAlike
{
    public interface IEncryptionAlgorithm
    {
        string Encrypt(string data, string key);
        string Decrypt(string data, string key);
    }
}