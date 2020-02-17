using A1.BankingApp.baseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.Repository
{
    public interface IAccountRepo
    {
        double AddAccount(Accounts newAccount);
        bool EditAccount(Accounts accounts);
        Accounts GetAccountDetailsByAccountNumber(double accountNumber);
        List<Accounts> GetAccountDetailsByUserName(string userName);
        bool DeleteAccount(double accountNumber);
    }
}
