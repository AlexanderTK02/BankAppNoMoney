using BankAppNoMoney.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankAppNoMoney.Models;

internal class AccountDetails
{
    internal string AccountName { get; set; } = "";
    public string AccountNumber { get; set; } = string.Empty;
    internal decimal StartingBalance { get; set; }
    internal AccountType AccountType { get; set; }
}
