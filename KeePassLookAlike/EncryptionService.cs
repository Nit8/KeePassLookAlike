namespace KeePassLookAlike
{
    public class EncryptionService : IEncryptionAlgorithm
    {
        private readonly IEncryptionAlgorithm _encryptionAlgorithm;

        public EncryptionService(IEncryptionAlgorithm encryptionAlgorithm)
        {
            _encryptionAlgorithm = encryptionAlgorithm;
        }

        public string Encrypt(string data, string key)
        {
            return _encryptionAlgorithm.Encrypt(data, key);
        }

        public string Decrypt(string data, string key)
        {
            return _encryptionAlgorithm.Decrypt(data, key);
        }
    }
 }
