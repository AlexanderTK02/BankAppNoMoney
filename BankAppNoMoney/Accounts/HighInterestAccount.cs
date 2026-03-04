using BankAppNoMoney.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney.Accounts;

internal class HighInterestAccount : AccountBase
{
    public HighInterestAccount(string accountName, string accountNumber) : base(0.15m, 10000m, accountName, accountNumber)
    {
    }

    internal override decimal Balance()
    {
        // GAMLA KODEN, INTE RADERAD FÖR ATT VISA VAD SOM ÄNDRATS
        //var t = BankTransactions.Sum(x => x.Amount);
        //return StartingBalance + t;

        return BankTransactions.Sum(x => x.Amount);
    }
}
