using System;
using System.Collections.Generic;
using System.Text;
using BankAppNoMoney.Base;

namespace BankAppNoMoney.Accounts;

internal class IskAccount : AccountBase
{
    public IskAccount(string accountName, string accountNumber) : base(0.5m, 500m, accountName, accountNumber)
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
