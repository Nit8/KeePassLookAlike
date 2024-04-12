using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KeePassLookAlike
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string dataFilePath = "password_data.edm";
            var aesEncryption = new AesEncryption("RXVWAACtujQcWQ=="); // Replace with a secure key generation process

            // Create an EncryptionService instance using the AesEncryption object
            var encryptionService = new EncryptionService(aesEncryption);

            var passwordManager = new PasswordManager(dataFilePath, encryptionService);


            int choice;

            do
            {
                Console.WriteLine("\nPassword Manager Menu:");
                Console.WriteLine("1. Create New Database");
                Console.WriteLine("2. Open Database");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter a strong master password: ");
                        string masterPassword = Console.ReadLine();
                        passwordManager.CreateNewDatabase(masterPassword);
                        Console.WriteLine("Database created successfully!");
                        break;
                    case 2:
                        Console.Write("Enter the master password: ");
                        masterPassword = Console.ReadLine();
                        var dataModel = passwordManager.OpenDatabase(masterPassword);

                        if (dataModel != null)
                        {
                            // Implement menu or logic to manage password entries here
                            // You can use dataModel.AddPasswordEntry, GetPasswordEntry, UpdatePasswordEntry, DeletePasswordEntry
                            Console.WriteLine("Database opened successfully!");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Exiting Password Manager.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

            } while (choice != 3);
        }
    }
}

