using System;
using System.Collections.Generic;
using System.Linq;
using Banking.Domain;
namespace Banking.App
{
    class Program
    {
        static List<BankAccount> accounts;
        static void Main(string[] args)
        {
            accounts = DataStorage.LoadAccounts();
            Console.WriteLine($"System loaded {accounts.Count} accounts found.");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("================= Banking System ================");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        DataStorage.SaveAccounts(accounts);
                        Console.WriteLine("Exiting... Data saved.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
                //module1: CreateAccount
                static void CreateAccount()
                {
                    Console.Clear();
                    Console.WriteLine("=== Open New Account===");
                    Console.Write("Enter Account Holder Name: ");
                    var name = Console.ReadLine();
                    Console.Write("Set a 4-digit PIN: ");
                    var pin = Console.ReadLine();
                    if (pin.Length != 4 || !pin.All(char.IsDigit))
                    {
                        Console.WriteLine("Invalid PIN format. Press Enter to return to main menu.");
                        Console.ReadLine();
                        return;
                    }
                    //generate unique accountid
                    Random rnd = new Random();
                    string accountId = rnd.Next(100000, 999999).ToString();
                    //creation of object and adding to list
                    var newAccount = new BankAccount(accountId, name, pin, 0);
                    // add the new account to the in-memory list, then save
                    accounts.Add(newAccount);
                    DataStorage.SaveAccounts(accounts);
                    Console.WriteLine($"Account created successfully! Your Account Number is: {accountId}");
                    Console.WriteLine("Press Enter to return to main menu.");
                    Console.ReadLine();
                }
                //module2: Login
                static void Login()
                {
                    Console.Clear();
                    Console.WriteLine("=== Account Login ===");
                    Console.Write("Enter Account Number: ");
                    var accNumber = Console.ReadLine();
                    var account = accounts.FirstOrDefault(a => a.AccountNumber == accNumber);
                    if (account == null)
                    {
                        Console.WriteLine("Account not found. Press Enter to return to main menu.");
                        Console.ReadLine();
                        return;
                    }
                    if (account.IsLocked)
                    {
                        Console.WriteLine("Account is locked due to multiple failed login attempts. Press Enter to return to main menu.");
                        Console.ReadLine();
                        return;
                    }
                    int attempts = 0;
                    while (attempts < 3)
                    {
                        Console.Write("Enter PIN: ");
                        var pin = Console.ReadLine();
                        if (pin == account.Pin)
                        {
                            AccountMenu(account);
                            return;
                        }
                        else
                        {
                            attempts++;
                            Console.WriteLine($"Incorrect PIN. {3 - attempts} attempts remaining.");
                        }
                    }
                    account.LockAccount();
                    DataStorage.SaveAccounts(accounts);
                    Console.WriteLine("Account locked due to multiple failed login attempts. Press Enter to return to main menu.");
                    Console.ReadLine();
                }
                static void AccountMenu(BankAccount account)
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"=== Welcome, {account.AccountHolderName} ===");
                        Console.WriteLine("1. View Balance");
                        Console.WriteLine("2. Deposit");
                        Console.WriteLine("3. Withdraw");
                        Console.WriteLine("4. View Transaction History");
                        Console.WriteLine("5. Logout");
                        Console.Write("Select an option: ");
                        var choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine($"Current Balance: {account.Balance:C}");
                                break;
                            case "2":
                                Console.Write("Enter deposit amount: ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                                {
                                    try
                                    {
                                        account.Deposit(depositAmount);
                                        DataStorage.SaveAccounts(accounts);
                                        Console.WriteLine("Deposit successful.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount.");
                                }
                                break;
                            case "3":
                                Console.Write("Enter withdrawal amount: ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                                {
                                    try
                                    {
                                        account.Withdraw(withdrawAmount);
                                        DataStorage.SaveAccounts(accounts);
                                        Console.WriteLine("Withdrawal successful.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid amount.");
                                }
                                break;
                            case "4":
                                Console.WriteLine("=== Transaction History ===");
                                foreach (var transaction in account.TransactionHistory)
                                {
                                    Console.WriteLine(transaction);
                                }
                                break;
                            case "5":
                                return;
                            default:
                                Console.WriteLine("Invalid option.");
                                break;
                        }
                        Console.WriteLine("Press Enter to continue.");
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}


