using System;
using System.Collections.Generic;
using System.Security.Cryptography;
namespace Banking.Domain
{
    public class BankAccount

    {
            public string AccountNumber { get; set; }
            public string AccountHolderName { get; set; }
            public string Pin { get; set; }
            public string PinHash { get; set; }
            public string PinSalt { get; set; }

            public decimal Balance { get; set; }
            public bool IsLocked { get; set; }
            public List<string> TransactionHistory { get; set; } = new List<string>();
            public BankAccount() { }
            public BankAccount(string accountNumber, string accountHolderName, string pin, decimal initialBalance)
            {
                AccountNumber = accountNumber;
                AccountHolderName = accountHolderName;
                SetPin(pin);
                Balance = initialBalance;
                IsLocked = false;
                AddTransaction("Account Created.");
            }
            public void SetPin(string pin)
            {
                if (string.IsNullOrEmpty(pin))
                    throw new ArgumentNullException("PIN cannot be empty.", nameof(pin));
                byte[] salt = new byte[16];
                RandomNumberGenerator.Fill(salt);
                byte[] hash;
                using (var pbkdf2 = new Rfc2898DeriveBytes(pin, salt, 100_100, HashAlgorithmName.SHA256))
                {
                    hash = pbkdf2.GetBytes(32);
                }
                PinSalt = Convert.ToBase64String(salt);
                PinHash = Convert.ToBase64String(hash);
                Pin = null;
            }
            public bool VerifyPin(string pin)
            {
                if (string.IsNullOrEmpty(PinHash) || string.IsNullOrEmpty(PinSalt))
                    return false;
                byte[] salt = Convert.FromBase64String(PinSalt);
                byte[] expectedHash = Convert.FromBase64String(PinHash);
                byte[] actualHash;
                using (var pbkdf2 = new Rfc2898DeriveBytes(pin, salt, 100_100, HashAlgorithmName.SHA256))
                {
                    actualHash = pbkdf2.GetBytes(expectedHash.Length);

                }
                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
            public void Deposit(decimal amount)
            {
                if (amount <= 0)
                    throw new ArgumentException("Deposit amount must be positive.");
                Balance += amount;
                AddTransaction($"Deposited: {amount:C}. New Balance: {Balance:C}");
            }
            public void Withdraw(decimal amount)
            {
                if (amount <= 0)
                    throw new ArgumentException("Withdrawal amount must be positive.");
                if (amount > Balance)
                    throw new InvalidOperationException("Insufficient funds.");
                Balance -= amount;
                AddTransaction($"Withdrew: {amount:C}. New Balance: {Balance:C}");
            }
            public void LockAccount()
            {
                IsLocked = true;
                AddTransaction("Account Locked:Too many failed PIN attempts");
            }
            private void AddTransaction(string details)
            {
                TransactionHistory.Add($"{DateTime.Now}: {details}");
            }
        }
    }
