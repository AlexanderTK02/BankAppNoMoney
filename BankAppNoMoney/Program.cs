using BankAppNoMoney.Accounts;
using BankAppNoMoney.Base;

namespace BankAppNoMoney;

internal class Program
{
    static void Main(string[] args)
    {
        new Bank().ShowBankMenu();
    }
}
