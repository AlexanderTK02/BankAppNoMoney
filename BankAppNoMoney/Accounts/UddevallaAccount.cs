using System;
using System.Collections.Generic;
using System.Text;
using BankAppNoMoney.Base;

namespace BankAppNoMoney.Accounts;

internal class UddevallaAccount : AccountBase
{                                                                 // Siffrorna nedan är:  Ränta och startbelopp 
    public UddevallaAccount(string accountName, string accountNumber) : base(0.03m, 500m, accountName, accountNumber)
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
