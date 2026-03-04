using BankAppNoMoney.Accounts;
using BankAppNoMoney.Base;
using BankAppNoMoney.Factories;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Data;
using System.Threading;
using BankAppNoMoney.Models;
using BankAppNoMoney.Types;

namespace BankAppNoMoney;
/*
Farzad code-review

ShowBankMenu() finns redan en evighetsloop, så att anropa metoden igen känns kanske onödigt  
| FIXAT! |

I CreateAccount() körs metoden om vid felaktig input, en loop hade nog räckt  
| FIXAT PÅ ETT ANNAT SÄTT! |

Villkoren i HandleAccounts() verkar bara kolla upp till val 4, men menyn har fler alternativ  
| FIXAT MEN PÅ ANNAT SÄTT, VAL 6 ÄR EXIT |

Thread.Sleep() används, värt att dubbelkolla att rätt namespace är med
| FIXAT MED NAMESPACE |

Blandning av ReadKey och ReadLine gör input-flödet lite ojämnt  
| VISSA VAL GÅR ÖVER 9 SÅ GÅR INTE ATT HA READKEY MEN SAMTIDIGT SÅ ÄR READKEY SMIDIGARE |

GetAccount() heter i singular men returnerar flera konton, kan vara lite missvisande
| FIXAT! |

Settern på Accounts är öppen, listan kan kanske ändras oavsiktligt
| FIXAT, TOG BORT SET VILKET FÖRHINDRAR ÄNDRINGAR UTOM ADD OCH REMOVE ACCOUNT |

I årssimuleringen hamnar ogiltig input direkt i logiken
| FIXAT MED NÅGRA IF-SATSER |

Random skapas i metoden, osäker om det är bästa stället
| METODEN ÄR SÅ LITEN ATT DET GÅR BRA. OM JAG SKA UTVECKLA METODEN MER I FRAMTIDEN SÅ ÄNDRAR JAG |

Simuleringen ändrar faktiskt saldot, beror på hur man tänkt att funktionen ska funka
| DET VAR MENINGEN ATT ÄNDRA SALDOT PÅRIKTIGT |

 */

internal class Bank
{
    /*Känns jättemycket och jättemånga rader i en och samma klass, hade kanske varit bättre att separera de till olika klasser*/
    /*Hade varit bättre om man tryckte på enter eller någon bokstav under tiden texten skrivs ut med delay inte räknades med, utan bara när hela texten hade skrivits ut. borde finnas någon liknande funktion.*/
    /*Vissa text som tex när man vill se alla konton, det ser jättefin ut att texten rad för rad skrivs ut automatiskt men i vissa fall tar det för långt att sitta och vänta tills texten skrivs ut.*/

    /*DramaticEffectLine($"{depositAmount} kr har lagts in i kontot.", 2);*/ /*Detta skrivs ut oavsett, även fast ett nummer är ett minus-siffra*/
    /*Konton skapas äen fast man bara skriver Enter, hur du tänkt något specifikt där?*/
    /**/

    // Fixat alla kommentarer, kommer separera dem om appen utvecklas vidare.

    internal List<AccountBase> Accounts { get; } = new();

    public Bank()
    {
        SeedAccounts(); // Skapar konton med slumpmässiga startbelopp när banken initieras
    }

    private void SeedAccounts()
    {
        var accountsToSeed = new List<AccountBase>
    {
        new BankAccount("Lönekonto", "1001"),
        new IskAccount("Investering", "2001"),
        new UddevallaAccount("Semesterkonto", "3001"),

        new HighYieldAccount("Sparkonto", "1002"),
        new BankAccount("Matkonto", "1003"),
        new HighYieldAccount("Fonder", "2002"),
        new HighInterestAccount("Aktier", "2003"),
        new UddevallaAccount("Bilkonto", "3002"),
        new HighInterestAccount("Buffert", "3003"),
        new BankAccount("Hushåll", "1004")
    };

        var random = new Random();

        foreach (var acc in accountsToSeed) // Loopar igenom varje konto i listan och sätter in ett slumpmässigt belopp.
        {
            acc.Deposit(random.Next(500, 10000));
            Accounts.Add(acc);
        }
    }

    public void ShowBankMenu() // Huvudmenyn som visas när programmet startar
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
            Console.Clear();

            switch (keyPress)
            {
                case '1':
                    CreateAccount();
                    break;
                case '2':
                    RemoveAccount();
                    break;
                case '3':
                    ShowAllAccounts();
                    break;
                case '4':
                    HandleAccounts();
                    break;
                case '5':
                    Environment.Exit(0);
                    break;
                default:
                    WrongInput("Ogiltigt val, försök igen.");
                    break;
            }
        }
    }

    private void CreateAccount()
    {
        string accountTypeTemp = null!;

        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine("Vilken typ av konto vill du skapa?", 2);
        Console.WriteLine();
        DramaticEffectLine("1. BankAccount", 2);       // Ränta = 1%
        DramaticEffectLine("2. IskAccount", 2);        // Ränta = 5%
        DramaticEffectLine("3. UddevallaAccount", 2);  // Ränta = 3%
        DramaticEffectLine("4. HighInterestAccount", 2);
        DramaticEffectLine("5. HighYieldAccount", 2);
        Console.WriteLine();
        DramaticEffectLine("6. Tillbaka till huvudmenyn", 2);

        char chosenTypeAcc = Console.ReadKey(true).KeyChar;
        Console.WriteLine();

        if (chosenTypeAcc == '1' || chosenTypeAcc == '2' || chosenTypeAcc == '3' || chosenTypeAcc == '4' || chosenTypeAcc == '5' || chosenTypeAcc == '6')
        {
            if (chosenTypeAcc == '1')
            {
                DramaticEffectLine("Kontotyp: BankAccount", 2);
                accountTypeTemp = "BankAccount";
            }
            else if (chosenTypeAcc == '2')
            {
                DramaticEffectLine("Kontotyp: IskAccount", 2);
                accountTypeTemp = "IskAccount";
            }
            else if (chosenTypeAcc == '3')
            {
                DramaticEffectLine("Kontotyp: UddevallaAccount", 2);
                accountTypeTemp = "UddevallaAccount";
            }
            else if (chosenTypeAcc == '4')
            {
                DramaticEffectLine("Kontotyp: HighInterestAccount", 2);
                accountTypeTemp = "HighInterestAccount";
            }
            else if (chosenTypeAcc == '5')
            {
                DramaticEffectLine("Kontotyp: HighYieldAccount", 2);
                accountTypeTemp = "HighYieldAccount";
            }
            else if (chosenTypeAcc == '6')
            {
                Console.Clear();
                return;
            }

            Console.WriteLine();
            Console.Write("Konto Namn: ");
            string accountName = Console.ReadLine()!;
            Console.WriteLine();

            if (string.IsNullOrEmpty(accountName) || accountName.Length < 2)
            {
                Console.Clear();
                WrongInput("Konto namn får inte vara tom eller mindre än 3 bokstäver lång");
                return;
            }

            Console.Write("Konto Nummer: ");
            string accountNumber = Console.ReadLine()!;
            Console.WriteLine();

            if (string.IsNullOrEmpty(accountNumber) || int.TryParse(accountNumber, out int results) == false || accountNumber.Length < 3)
            {
                Console.Clear();
                WrongInput("Konto nummer får inte innehålla bokstäver, vara tom eller vara mindre än 3 siffror lång");
                return;
            }


            var accountDetails = new AccountDetails()
            {
                AccountName = accountName,
                AccountNumber = accountNumber,
                StartingBalance = 0m,
                AccountType = chosenTypeAcc == '1' ? AccountType.BankAccount
                            : chosenTypeAcc == '2' ? AccountType.IskAccount
                            : chosenTypeAcc == '3' ? AccountType.UdevallaAccount
                            : chosenTypeAcc == '4' ? AccountType.HighInterestAccount
                            : AccountType.HighYieldAccount
            };

            AccountBase newAccount = AccountFactory.CreateAccount(accountDetails);


            if (accountDetails.StartingBalance > 0)
            {
                newAccount.Deposit(accountDetails.StartingBalance);
            }

            AddAccount(newAccount);

            Console.WriteLine();
            DramaticEffectLine("Kontot har skapats!", 2);
            Console.WriteLine();
            DramaticEffectLine($" Konto Typ: {accountTypeTemp}", 2);
            DramaticEffectLine($" Konto Namn: {accountName}", 2);
            DramaticEffectLine($" Konto Nummer: {accountNumber}", 2);
            Thread.Sleep(2000);
            Console.Clear();
        }
        else
        {
            WrongInput("Ogiltigt val, försök igen.");
            return;
        }
    }

    private void RemoveAccount()
    {
        if (Accounts.Count == 0)
        {
            WrongInput("Inga konton att ta bort.");
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
            WrongInput("Ogiltigt val.");
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
            //DramaticEffectLine("-----------------------------------------------------------------------------------------", 0);
            //foreach (var account in Accounts)
            //{
            //    DramaticEffectLine($"| Konto Typ: | {account.GetType().Name} |", 0);
            //    DramaticEffectLine($"| Konto Namn: {account.AccountName} | Konto Nummer: {account.AccountNumber} | Saldo: {Math.Round(account.Balance(), 2)} SEK |", 0);
            //    DramaticEffectLine("-----------------------------------------------------------------------------------------", 0);
            //    Thread.Sleep(150);
            //}

            DramaticEffectLine("----------------------------------------------------------------------------------", 0);
            DramaticEffectLine($"| {"Typ",-20} | {"Namn",-20} | {"Nummer",-10} | {"Saldo",-15}     |", 0);
            DramaticEffectLine("----------------------------------------------------------------------------------", 0);

            foreach (var account in Accounts)
            {
                DramaticEffectLine(
                    $"| {account.GetType().Name,-20} | {account.AccountName,-20} | {account.AccountNumber,-10} | {account.Balance(),-15:F2} SEK |",
                    0
                );

                Thread.Sleep(150);
            }

            DramaticEffectLine("----------------------------------------------------------------------------------", 0);
        }
        Console.WriteLine();
        DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
        Console.ReadKey(true);
        Console.Clear();
    }

    private void HandleAccounts()
    {
        if (Accounts.Count == 0)
        {
            WrongInput("Inga konton att hantera.");
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

        Console.WriteLine();
        if (!int.TryParse(Console.ReadLine(), out int accountIndex) || accountIndex < 1 || accountIndex > Accounts.Count)
        {
            WrongInput("Något gick fel");
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
            DramaticEffectLine("4. Simulera 1 års ränta", 2);
            DramaticEffectLine("5. Visa transaktioner", 2);
            DramaticEffectLine("6. Tillbaka till huvudmenyn", 2);

            char chosenOption = Console.ReadKey(true).KeyChar;
            Console.Clear();

            switch (chosenOption)
            {
                case '1':
                    DepositMoney(selectedAccount);
                    break;

                case '2':
                    WithdrawMoney(selectedAccount);
                    break;

                case '3':
                    DramaticEffectLine("Svensk Bank", 2);
                    Console.WriteLine();
                    DramaticEffectLine($"Saldo: {Math.Round(selectedAccount.Balance(), 2)} kr", 2);
                    Console.WriteLine();
                    break;

                case '4':
                    SimulationInterest(selectedAccount);
                    break;

                case '5':
                    ShowTransactions(selectedAccount);
                    break;

                case '6':
                    exitAccountMenu = true;
                    break;
            }

            if (chosenOption < '1' || chosenOption > '5')
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

    private void ShowTransactions(AccountBase selectedAccount)
    {
        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine("Transaktioner:", 2);

        var transaktioner = selectedAccount.GetBankTransactions();
        Console.WriteLine();
        Console.WriteLine("------------------------------------------------------------------------------");

        foreach (var tA in transaktioner)
        {
            Console.WriteLine();
            Console.WriteLine($"{tA.TransactionalDate:yy-MM-dd} - {Math.Round(tA.Amount, 2)} kr");
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------");
            Thread.Sleep(10);
        }
    }

    private void SimulationInterest(AccountBase selectedAccount)
    {
        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();

        Console.Write("Ange belopp för insättning under året: ");
        decimal simDeposit = decimal.TryParse(Console.ReadLine(), out decimal tempDeposit) ? tempDeposit : 0m;
        Console.WriteLine();

        if (simDeposit < 0)
        {
            WrongInput("Antal får inte vara mindre än noll");
            return;
        }

        Console.Write("Ange antal gånger beloppet ska sättas in under simuleringen: ");
        int simTimes = int.TryParse(Console.ReadLine(), out int tempDeposits) ? tempDeposits : 0;

        Console.Clear();
        if (simTimes <= 0)
        {
            WrongInput("Antal får inte vara noll eller mindre");
            return;
        }
        if (simTimes > 365)
        {
            WrongInput("Antal får inte vara över 365");
            return;
        }

        decimal tempBalance = selectedAccount.Balance();
        decimal sumOfSim = simDeposit * simTimes;

        selectedAccount.SimulateYear(simDeposit, simTimes);
        decimal sumOfSimInterest = selectedAccount.Balance() - (tempBalance + sumOfSim);

        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine("Ett år har simulerats!", 2);
        Console.WriteLine("------------------------------------------------------");
        DramaticEffectLine($"| Start Saldo:                 |  {Math.Round(tempBalance, 2)} kr", 2);
        Console.WriteLine("------------------------------------------------------");
        DramaticEffectLine($"| Total summa av insättningar: |  {sumOfSim}", 2);
        Console.WriteLine("------------------------------------------------------");
        DramaticEffectLine($"| Total vinst av ränta:        |  {Math.Round(sumOfSimInterest, 2)}", 2);
        Console.WriteLine("------------------------------------------------------");
        DramaticEffectLine($"| Uppdaterad Saldo:            |  {Math.Round(selectedAccount.Balance(), 2)} kr", 2);
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine();
    }

    private void WithdrawMoney(AccountBase selectedAccount)
    {
        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        Console.Write("Ange belopp att ta ut: ");

        if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
        {
            bool transactionWork = selectedAccount.Withdraw(withdrawAmount);

            if (transactionWork)
            {
                Console.WriteLine();
                DramaticEffectLine($"{withdrawAmount} kr har tagits ut från kontot.", 2);
                Console.WriteLine();
                DramaticEffectLine($"Uppdaterad Saldo: {Math.Round(selectedAccount.Balance(), 2)}", 2);
                Console.WriteLine();
            }

        }
    }

    private void DepositMoney(AccountBase selectedAccount)
    {
        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        Console.Write("Ange belopp att sätta in: ");

        if ((Decimal.TryParse(Console.ReadLine(), out decimal depositAmount)) || depositAmount > 0)
        {
            selectedAccount.Deposit(depositAmount);
            Console.WriteLine();
            DramaticEffectLine($"{depositAmount} kr har lagts in i kontot.", 2);
            Console.WriteLine();
            DramaticEffectLine($"Uppdaterad Saldo: {Math.Round(selectedAccount.Balance(), 2)}", 2);
            Console.WriteLine();
        }
        else
        {
            Console.Clear();
            DramaticEffectLine("Svensk Bank", 2);
            Console.WriteLine();
            DramaticEffectLine("Felaktigt värde, får inte vara mindre än 1", 2);
            Console.WriteLine();
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

    internal List<AccountBase> GetAccounts()
    {
        return Accounts;
    }

    /// <summary>
    /// Displays a message to the user with a dramatic effect, indicating an incorrect input, and prompts the user to
    /// continue.
    /// </summary>
    /// <remarks>This method clears the console, presents the specified message with a dramatic effect, and
    /// waits for the user to press a key before clearing the console again. Intended for use when notifying the user of
    /// invalid or unexpected input.</remarks>
    /// <param name="textInput">The message to display to the user regarding the incorrect input.</param>
    internal static void WrongInput(string textInput)
    {
        Console.Clear();
        DramaticEffectLine("Svensk Bank", 2);
        Console.WriteLine();
        DramaticEffectLine(textInput, 2);
        Console.WriteLine();
        DramaticEffectLine("Tryck på någon knapp för att fortsätta", 2);
        Console.ReadKey(true);
        Console.Clear();
    }

    /// <summary>
    /// Console.WriteLine with a dramatic effect by printing each character with a delay in between.
    /// </summary>
    /// <remarks>This method iterates through each character in the provided string, printing it to the console with a specified delay between characters. 
    /// After printing the entire string, it moves to a new line. This is intended to create a dramatic effect when displaying messages to the user.</remarks>
    /// <param name="text">The string to send into the method and produce a dramatic effect.</param>
    /// <param name="delay">Delay in milliseconds between each char in the string.</param>
    static void DramaticEffectLine(string text, int delay)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }
}
