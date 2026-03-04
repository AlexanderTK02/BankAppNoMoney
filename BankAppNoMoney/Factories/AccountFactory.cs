using BankAppNoMoney.Accounts;
using BankAppNoMoney.Base;
using BankAppNoMoney.Models;
using BankAppNoMoney.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney.Factories;

internal static class AccountFactory
{
    internal static AccountBase CreateAccount(AccountDetails accountDetails)
    {
        switch (accountDetails.AccountType)
        {
            case AccountType.BankAccount:
                return new BankAccount(accountDetails.AccountName, 
                    accountDetails.AccountNumber);
            case AccountType.IskAccount:
                return new IskAccount(accountDetails.AccountName,
                    accountDetails.AccountNumber);
            case AccountType.UdevallaAccount:
                return new UddevallaAccount(accountDetails.AccountName,
                    accountDetails.AccountNumber);
            case AccountType.HighInterestAccount:
                return new HighInterestAccount(accountDetails.AccountName,
                    accountDetails.AccountNumber);
            case AccountType.HighYieldAccount:
                return new HighYieldAccount(accountDetails.AccountName,
                    accountDetails.AccountNumber);
            default:
                throw new NotImplementedException();
        }
    }
}
