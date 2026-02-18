using System;
using System.Collections.Generic;
using System.Text;
using BankAppNoMoney.Base;

namespace BankAppNoMoney.Accounts;

internal class UddevallaAccount : AccountBase
{
    public UddevallaAccount(string accountName, string accountNumber) : base(accountName, accountNumber)
    {
    }

    internal override decimal Balance()
    {
        var t = BankTransactions.Sum(x => x.Amount);
        return StartingBalance + t;

    }
}
