using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney.Base;

internal abstract class AccountBase
{
    internal Guid Id { get; set; } = Guid.NewGuid();
    internal decimal StartingBalance { get; set; } = 500;
    internal string AccountName { get; set; } = "";
    internal string AccountNumber { get; set; } = "";
    internal decimal InterestRate { get; set; } = 0;

    public AccountBase(string accountName, string accountNumber)
    {
        AccountName = accountName;
        AccountNumber = accountNumber;
    }

    protected List<BankTransaction> BankTransactions = new List<BankTransaction>();

    internal abstract decimal Balance();

    internal virtual void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Transaction can't be completed, amount cant be 0 or less.");
        }
        else
        {
            var t = new BankTransaction
            {
                Amount = amount,
                TransactionalDate = DateTime.Now
            };

        BankTransactions.Add(t);
        }
    }

    internal virtual bool Withdraw(decimal amount, bool transactionWork)
    {

        if (amount > Balance())
        {
            Console.WriteLine();
            Console.WriteLine("Transaction can't be completed, balance is too low.");
            Console.WriteLine();
        }
        else
        {
            var t = new BankTransaction
            {
                Amount = -amount,
                TransactionalDate = DateTime.Now
            };

            BankTransactions.Add(t);
            transactionWork = true;
        }

        return transactionWork;

    }
}
