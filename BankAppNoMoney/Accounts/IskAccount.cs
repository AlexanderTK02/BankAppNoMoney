using System;
using System.Collections.Generic;
using System.Text;
using BankAppNoMoney.Base;

namespace BankAppNoMoney.Accounts;

internal class IskAccount : AccountBase
{                                                                 // Siffrorna nedan är:  Ränta och startbelopp 
    public IskAccount(string accountName, string accountNumber) : base(0.05m, 5000m, accountName, accountNumber)
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
