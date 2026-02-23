using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney.Base;

internal abstract class AccountBase
{
    internal Guid Id { get; set; } = Guid.NewGuid();
    internal string AccountName { get; set; } = "";
    internal string AccountNumber { get; set; } = "";
    public decimal InterestRate { get; protected set; }

    protected AccountBase(decimal interestRate, decimal startingBalance,
                      string accountName, string accountNumber)
    {
        InterestRate = interestRate;
        AccountName = accountName;
        AccountNumber = accountNumber;

        BankTransactions.Add(new BankTransaction
        {
            Amount = startingBalance,
            TransactionalDate = DateTime.Now
        });
    }

    protected List<BankTransaction> BankTransactions = new List<BankTransaction>();

    public IEnumerable<BankTransaction> GetBankTransactions()
    {
        return BankTransactions;
    }

    internal abstract decimal Balance();

    //internal abstract List<BankTransaction> GetBankTransactions();

    internal virtual void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Transaktionen kunde inte slutföras, belopp får inte vara 0 eller mindre");
        }
        else
        {
            var transaction = new BankTransaction
            {
                Amount = amount,
                TransactionalDate = DateTime.Now
            };

        BankTransactions.Add(transaction);
        }
    }

    internal virtual bool Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine();
            Console.WriteLine("Beloppet måste vara större än noll.");
            Console.WriteLine();
            return false;
        }
        
        if (amount > Balance())
        {
            Console.WriteLine();
            Console.WriteLine("Transaktionen kunde inte slutföras, kontosaldot är för lågt.");
            Console.WriteLine();
            return false;
        }
        
        var transaction = new BankTransaction
        {
            Amount = -amount,
            TransactionalDate = DateTime.Now
        };

        BankTransactions.Add(transaction);
        return true;

    }

                // Metod för att simulera ett år av ränta och insättningar
    internal void SimulateYear(decimal depositAmount, int numberOfDeposits)
    {
        decimal dailyInterestRate = InterestRate / 365m;  // Tar årsräntan och delar den på 365 för att få daglig ränta
        int depositInterval = 365 / numberOfDeposits;   // Beräknar hur ofta insättningarna ska göras under året

        DateTime startDate = DateTime.Now;

        for (int day = 1; day <= 365; day++)
        {
            decimal currentBalance = Balance();

            // Beräknar räntan för den aktuella dagen baserat på det nuvarande saldot.
            decimal interestForTheDay = currentBalance * dailyInterestRate;

            // Lägger till räntan som en transaktion om den är större än 0
            if (interestForTheDay > 0)
            {
                BankTransactions.Add(new BankTransaction
                {
                    Amount = interestForTheDay,
                    TransactionalDate = startDate.AddDays(day)
                });
            }

            if (day % depositInterval == 0)
            {
                BankTransactions.Add(new BankTransaction
                {
                    Amount = depositAmount,
                    TransactionalDate = startDate.AddDays(day)
                });
            }
        }
    }
}
