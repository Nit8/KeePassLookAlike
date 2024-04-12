using System.Security.Cryptography;
using System.Text.Json;

namespace KeePassLookAlike
{
    public class PasswordManager
    {
        private readonly string _dataFilePath;
        private readonly EncryptionService _encryptionService;

        public PasswordManager(string dataFilePath, EncryptionService encryptionService)
        {
            _dataFilePath = dataFilePath;
            _encryptionService = encryptionService;
        }

        public void CreateNewDatabase(string masterPassword)
        {
            // Validate master password strength (optional)
            // You can implement your own password strength check here

            // Generate a random salt
            byte[] salt = GenerateSalt();

            // Hash the master password with the salt
            string masterPasswordHash = HashPassword(masterPassword, salt);

            // Create a new EncryptionDataModel object
            var dataModel = new EncryptionDataModel(_encryptionService)
            {
                MasterPasswordSalt = salt
            };

            // Set the hashed password internally within the data model
            dataModel.SetHashedPassword(masterPasswordHash);

            // Encrypt the data model and serialize it to JSON
            string jsonData = JsonSerializer.Serialize(dataModel);
            string encryptedData = _encryptionService.Encrypt(jsonData, masterPassword);

            // Write the encrypted data to a file with .edm extension
            File.WriteAllText(_dataFilePath + ".edm", encryptedData);
        }

        public EncryptionDataModel OpenDatabase(string masterPassword)
        {
            if (!File.Exists(_dataFilePath + ".edm"))
            {
                Console.WriteLine("Error: Database file not found.");
                return null;
            }

            // Read the encrypted data from the file
            string encryptedData = File.ReadAllText(_dataFilePath + ".edm");

            // Decrypt the data using the provided master password
            string jsonData = _encryptionService.Decrypt(encryptedData, masterPassword);

            // Deserialize the JSON data back to an EncryptionDataModel object
            return JsonSerializer.Deserialize<EncryptionDataModel>(jsonData);
        }

        private byte[] GenerateSalt()
        {
            // Generate a random byte array of appropriate length (e.g., 32 bytes)
            byte[] salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private string HashPassword(string password, byte[] salt)
        {
            // Use a strong hashing algorithm like bcrypt or scrypt (external library needed)
            // This example uses System.Security.Cryptography.Rfc2898DeriveBytes (for PB
           
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }
    }
}
