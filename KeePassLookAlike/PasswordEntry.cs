namespace KeePassLookAlike
{
    public class PasswordEntry
    {
        public string Website { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // Encrypted password
        public string Notes { get; set; } // Optional
    }
}