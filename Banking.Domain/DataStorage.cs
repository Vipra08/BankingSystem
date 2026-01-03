using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Banking.Domain
{
    public static class DataStorage
    
        {
            // store the data file next to the app binaries so it's written to the running app folder
            private static string filePath = Path.Combine(AppContext.BaseDirectory, "bank_data.json");
            public static void SaveAccounts(List<BankAccount> accounts)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(accounts, options);
                    File.WriteAllText(filePath, jsonString);
                }
                catch (Exception)
                {
                    // In a simple console app we silently ignore save errors (could be logged)
                }
            }
            public static List<BankAccount> LoadAccounts()
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        return new List<BankAccount>();
                    }
                    string jsonString = File.ReadAllText(filePath);
                    if (string.IsNullOrWhiteSpace(jsonString))
                    {
                        return new List<BankAccount>();
                    }
                    var result = JsonSerializer.Deserialize<List<BankAccount>>(jsonString);
                    return result ?? new List<BankAccount>();
                }
                catch (Exception)
                {
                    // If the file is corrupt or unreadable, return an empty list to allow the app to continue
                    return new List<BankAccount>();
                }
            }
        }
    }
