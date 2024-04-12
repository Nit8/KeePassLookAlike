using Newtonsoft.Json.Linq;
using System.Runtime.Intrinsics.X86;
using System;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KeePassLookAlike
{
    public class EncryptionDataModel
    {
        public string MasterPasswordHash { get; set; }
        public byte[] MasterPasswordSalt { get; set; }
        private readonly EncryptionService _encryptionService;

        public List<PasswordEntry> PasswordEntries { get; private set; }

        public EncryptionDataModel( EncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            PasswordEntries = new List<PasswordEntry>();
        }

        private byte[] GenerateSalt()
        {
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
            // This example uses System.Security.Cryptography.Rfc2898DeriveBytes (for PBKDF2)
            // This requires .NET 6 or later
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }

        public void SetHashedPassword(string hashedPassword)
        {
            MasterPasswordHash = hashedPassword;
        }

        // Methods for managing password entries (implement these methods as needed)
        public void AddPasswordEntry(string website, string username, string password, string notes)
        {
            // Encrypt the password before adding it to the list
            string encryptedPassword = EncryptPassword(password);
            PasswordEntries.Add(new PasswordEntry
            {
                Website = website,
                Username = username,
                Password = encryptedPassword,
                Notes = notes
            });
        }

        public PasswordEntry GetPasswordEntry(string website)
        {
            return PasswordEntries.FirstOrDefault(entry => entry.Website == website);
        }

        public void UpdatePasswordEntry(string website, string newUsername, string newPassword, string newNotes)
        {
            // Find the password entry
            var entry = PasswordEntries.FirstOrDefault(entry => entry.Website == website);

            if (entry != null)
            {
                entry.Username = newUsername;
                entry.Password = EncryptPassword(newPassword); // Encrypt new password
                entry.Notes = newNotes;
            }
        }

        public void DeletePasswordEntry(string website)
        {
            var entry = PasswordEntries.FirstOrDefault(entry => entry.Website == website);

            if (entry != null)
            {
                PasswordEntries.Remove(entry);
            }
        }

        private string EncryptPassword(string password)
        {
            return _encryptionService.Encrypt(password, MasterPasswordHash);
        }
    }
}

