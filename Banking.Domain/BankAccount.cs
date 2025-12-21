using System;
using System.Collections.Generic;
namespace Banking.Domain
{
    public class BankAccount

    {
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string Pin { get; set; }
        public decimal Balance { get; private set; }
        public bool IsLocked { get; set; }
        public List<string> TransactionHistory { get; private set; }=new List<string>();
        public BankAccount() { }
        public BankAccount(string accountNumber, string accountHolderName, string pin, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            AccountHolderName = accountHolderName;
            Pin = pin;
            Balance = 0;
            IsLocked = false;
            AddTransaction("Account Created.");
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
