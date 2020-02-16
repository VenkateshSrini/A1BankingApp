using A1.BankingApp.baseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.Repository
{
    public interface IAccountRepo
    {
        int AddAccount(Accounts newAccount);
        bool EditAccount(Accounts accounts);
        Accounts GetAccountDetailsByAccountNumber(int accountNumber);
        List<Accounts> GetAccountDetailsByUserName(string userName);
        bool DeleteAccount(int accountNumber);
    }
}
