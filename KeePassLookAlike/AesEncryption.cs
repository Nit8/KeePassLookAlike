
using System.Security.Cryptography;
using System.Text;

namespace KeePassLookAlike
{
    public class AesEncryption : IEncryptionAlgorithm
    {
        private readonly byte[] _key; // Replace with a secure key generation process

        public AesEncryption(string key)
        {
            // You'll need to implement secure key generation and derivation from the password
            // This is a placeholder for demonstration purposes
            _key = Encoding.UTF8.GetBytes(key); // Not recommended for real use
        }

        public string Encrypt(string data, string key)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;

                // Generate a random IV
                byte[] iv = new byte[aes.BlockSize / 8];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(iv);
                }

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            // Write the IV to the beginning of the stream
                            memoryStream.Write(iv, 0, iv.Length);

                            byte[] cipherText = Encoding.UTF8.GetBytes(data);
                            cryptoStream.Write(cipherText, 0, cipherText.Length);
                        }

                        // Convert the entire memory stream to a base64 string
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        public string Decrypt(string data, string key)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.BlockSize = 128;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.Mode = CipherMode.CBC;

                byte[] dataBytes = Convert.FromBase64String(data);

                using (var decryptor = aes.CreateDecryptor())
                {
                    using (var memoryStream = new MemoryStream(dataBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] iv = new byte[aes.BlockSize / 8];
                            int ivRead = cryptoStream.Read(iv, 0, iv.Length);

                            if (ivRead != iv.Length)
                            {
                                throw new Exception("Failed to read the full IV from the encrypted data.");
                            }

                            aes.IV = iv;

                            byte[] decryptedBytes = new byte[dataBytes.Length - iv.Length];
                            int decryptedByteCount = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);

                            return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }
    }

}
