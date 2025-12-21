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
        private static string filePath = "bank_data.json";
        public static void SaveAccounts(List<BankAccount> accounts)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(accounts, options);
            File.WriteAllText(filePath, jsonString);
        }
        public static List<BankAccount> LoadAccounts()
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
            return JsonSerializer.Deserialize<List<BankAccount>>(jsonString);
        }
    }
}
