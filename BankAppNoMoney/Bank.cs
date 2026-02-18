using BankAppNoMoney.Accounts;
using BankAppNoMoney.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney;

internal class Bank
{
    internal List<AccountBase> Accounts { get; set; } = new List<AccountBase>();

    public Bank()
    {
        SeedAccounts();
    }

    private void SeedAccounts()
    {
        var accountsToSeed = new List<AccountBase>
    {
        new BankAccount("Lönekonto", "1001"),
        new IskAccount("Investering", "2001"),
        new UddevallaAccount("Semesterkonto", "3001"),

        new BankAccount("Sparkonto", "1002"),
        new BankAccount("Matkonto", "1003"),
        new IskAccount("Fonder", "2002"),
        new IskAccount("Aktier", "2003"),
        new UddevallaAccount("Bilkonto", "3002"),
        new UddevallaAccount("Buffert", "3003"),
        new BankAccount("Hushåll", "1004")
    };

        var random = new Random();

        foreach (var acc in accountsToSeed)
        {
            acc.Deposit(random.Next(500, 10000));
            Accounts.Add(acc);
        }
    }

    public void ShowBankMenu()
    {
        while (true)
        {

        DramaticEffectLine("Välkommen till Svensk Bank!", 5);
        Console.WriteLine();
        DramaticEffectLine("1. Skapa konto", 5);
        DramaticEffectLine("2. Ta bort konto", 5);
        DramaticEffectLine("3. Visa alla konton", 5);
        DramaticEffectLine("4. Hantera konton", 5);
        DramaticEffectLine("5. Avsluta", 5);

        char keyPress = Console.ReadKey(true).KeyChar;

        switch (keyPress)
            {
            case '1':
                Console.Clear();
                CreateAccount();
                break;
            case '2':
                Console.Clear();
                RemoveAccount();
                break;
            case '3':
                Console.Clear();
                ShowAllAccounts();
                break;
            case '4':
                Console.Clear();
                HandleAccounts();
                break;
            case '5':
                Environment.Exit(0);
                break;
            default:
                Console.Clear();
                Console.WriteLine("Ogiltigt val, försök igen.");
                Console.WriteLine("Tryck på någon knapp för att fortsätta");
                Console.ReadKey(true);
                Console.Clear();
                ShowBankMenu();
                break;
            }
        }
    }

    private void CreateAccount()
    {
        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine("Vilken typ av konto vill du skapa?", 2);
        Console.WriteLine();
        DramaticEffectLine("1. BankAccount", 2);
        DramaticEffectLine("2. IskAccount", 2);
        DramaticEffectLine("3. UddevallaAccount", 2);
        Console.WriteLine();

        string accountType = Console.ReadLine()!;
        Console.WriteLine();
        if (accountType == "1" || accountType == "2" || accountType == "3")
        {
            Console.Write("Konto Namn: ");
            string accountName = Console.ReadLine()!;
            Console.WriteLine();

            Console.Write("Konto Nummer: ");
            string accountNumber = Console.ReadLine()!;
            Console.WriteLine();

            AccountBase newAccount;

            switch (accountType)
            {
                case "1":
                    newAccount = new BankAccount(accountName, accountNumber);
                    break;
                case "2":
                    newAccount = new IskAccount(accountName, accountNumber);
                    break;
                case "3":
                    newAccount = new UddevallaAccount(accountName, accountNumber);
                    break;
                default:
                    Console.Clear();
                    DramaticEffectLine("Svensk Bank", 2);
                    Console.WriteLine();
                    DramaticEffectLine("Något gick fel, vänligen försök igen.", 2);
                    Console.WriteLine();
                    DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
                    Console.ReadKey(true);
                    Console.Clear();
                    CreateAccount();
                    return;
            }

            AddAccount(newAccount);
            Console.WriteLine($"Kontot har skapats: {accountName} - {accountNumber}");
            Thread.Sleep(2000);
            Console.Clear();
        }
        else
        {
            Console.Clear();
            DramaticEffectLine("Svensk Bank", 2);
            Console.WriteLine();
            DramaticEffectLine("Ogiltigt val, försök igen.", 2);
            Console.WriteLine();
            DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
            Console.ReadKey(true);
            Console.Clear();
            CreateAccount();
            return;
        }

    }

    private void RemoveAccount()
    {
        if (Accounts.Count == 0)
        {
            DramaticEffectLine("Svensk Bank", 2);
            Console.WriteLine();
            DramaticEffectLine("Inga konton att ta bort.", 2);
            Console.WriteLine();
            DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
            Console.ReadKey(true);
            Console.Clear();
            return;
        }

        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine("Välj konto att ta bort:", 2);
        Console.WriteLine();

        for (int i = 0; i < Accounts.Count; i++)
        {
            DramaticEffectLine($"{i + 1}. {Accounts[i].AccountName} - {Accounts[i].AccountNumber}", 2);
        }

        if (!int.TryParse(Console.ReadLine(), out int choice)
            || choice < 1
            || choice > Accounts.Count)
        {
            Console.Clear();
            DramaticEffectLine("Svensk Bank", 2);
            Console.WriteLine();
            DramaticEffectLine("Ogiltigt val.", 2);
            Console.WriteLine();
            DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
            Console.ReadKey(true);
            Console.Clear();
            return;
        }

        var selectedAccount = Accounts[choice - 1];

        RemoveAccount(selectedAccount.Id);

        Console.WriteLine();
        DramaticEffectLine("Kontot har tagits bort.", 2);
        Console.WriteLine();
        DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
        Console.ReadKey(true);
        Console.Clear();
    }

    private void ShowAllAccounts()
    {
        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine("Visar alla konton:", 2);
        Console.WriteLine();
        if (Accounts.Count == 0)
        {
            DramaticEffectLine("Inga konton hittades.", 2);
            Console.WriteLine();
        }
        else
        {
            DramaticEffectLine("-----------------------------------------------------------------------------------------", 0);
            foreach (var account in Accounts)
            {
                DramaticEffectLine($"| Konto Typ: | {account.GetType().Name} |", 2);
                DramaticEffectLine($"| Konto Namn: {account.AccountName} | Konto Nummer: {account.AccountNumber} | Saldo: {account.Balance()} SEK |", 2);
                DramaticEffectLine("-----------------------------------------------------------------------------------------", 0);
            }
        }
        DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
        Console.ReadKey(true);
        Console.Clear();
    }

    private void HandleAccounts()
    {
        if (Accounts.Count == 0)
        {
            DramaticEffectLine("Svensk Bank", 2);
            Console.WriteLine();
            DramaticEffectLine("Inga konton att hantera.", 2);
            Console.WriteLine();
            DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
            Console.ReadKey(true);
            Console.Clear();
            return;
        }

        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine("Välj ett konto att hantera:", 2);
        Console.WriteLine();

        for (int i = 0; i < Accounts.Count; i++)
        {
            DramaticEffectLine($"{i + 1}. {Accounts[i].AccountName} - {Accounts[i].AccountNumber}", 2);
        }

        if (!int.TryParse(Console.ReadLine(), out int accountIndex) || accountIndex < 1 || accountIndex > Accounts.Count)
        {
            Console.Clear();
            DramaticEffectLine("Svensk Bank", 2);
            Console.WriteLine();
            DramaticEffectLine("Ogiltigt val, försök igen.", 2);
            Console.WriteLine();
            DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
            Console.ReadKey(true);
            Console.Clear();
            return;
        }

        var selectedAccount = Accounts[accountIndex - 1];
        bool exitAccountMenu = false;

        while (exitAccountMenu == false)
        {
            Console.Clear();
            DramaticEffectLine("Svensk Bank", 2);
            Console.WriteLine();
            DramaticEffectLine($"Hantera konto: {selectedAccount.AccountName} - {selectedAccount.AccountNumber}", 2);
            Console.WriteLine();
            DramaticEffectLine("1. Sätt in pengar", 2);
            DramaticEffectLine("2. Ta ut pengar", 2);
            DramaticEffectLine("3. Visa saldo", 2);
            DramaticEffectLine("4. Tillbaka till huvudmenyn", 2);

            char chosenOption = Console.ReadKey(true).KeyChar;
            Console.Clear();

            switch (chosenOption)
            {
                case '1':
                    DramaticEffectLine("Svensk Bank", 2);
                    Console.WriteLine();
                    DramaticEffectLine("Ange belopp att sätta in:", 2);
                    Console.WriteLine();
                    if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                    {
                        selectedAccount.Deposit(depositAmount);
                        Console.WriteLine();
                        DramaticEffectLine($"{depositAmount} kr har lagts in i kontot.", 2);
                        Console.WriteLine();
                        DramaticEffectLine($"Uppdaterad Saldo: {selectedAccount.Balance()}", 2);
                        Console.WriteLine();
                    }
                        break;

                case '2':
                    bool transactionWork = false;

                    DramaticEffectLine("Svensk Bank", 2);
                    Console.WriteLine();
                    DramaticEffectLine("Ange belopp att ta ut:", 2);
                    Console.WriteLine();
                    if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                    {
                        selectedAccount.Withdraw(withdrawAmount, transactionWork);

                        if (transactionWork == true)
                        {
                            Console.WriteLine();
                            DramaticEffectLine($"{withdrawAmount} kr har tagits ut från kontot.", 2);
                            Console.WriteLine();
                            DramaticEffectLine($"Uppdaterad Saldo: {selectedAccount.Balance()}", 2);
                            Console.WriteLine();
                        }

                    }
                    break;

                case '3':
                    DramaticEffectLine("Svensk Bank", 2);
                    Console.WriteLine();
                    DramaticEffectLine($"Saldo: {selectedAccount.Balance()} kr", 2);
                    Console.WriteLine();
                    break;

                case '4':
                    exitAccountMenu = true;
                    break;
            }

            if (chosenOption < '1' || chosenOption > '3')
            {
                Console.Clear();
            }
            else
            {
            DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
            Console.ReadKey(true);
            Console.Clear();
            }
        }
    }


    internal void AddAccount(AccountBase account)
    {
        Accounts.Add(account);
    }

    internal void RemoveAccount(Guid accountId)
    {
        var account = Accounts.FirstOrDefault(x => x.Id == accountId);
        if (account != null)
        {
            Accounts.Remove(account);
        }
    }

    internal List<AccountBase> GetAccount()
    {
        return Accounts;
    }

    static void DramaticEffectLine(string text, int delay)  // Writes text with a dramatic effect
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }
}
