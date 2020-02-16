using A1.BankingApp.baseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace A1.BankingApp.Repository
{

    public class AccountRepository: IAccountRepo, IDisposable
    {
        protected Dictionary<int, Accounts> AccountRepo = new Dictionary<int, Accounts>();

        public int AddAccount(Accounts newAccount)
        {
            var temp = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var accountNumber = int.Parse(Regex.Replace(temp, "[a-zA-Z]", string.Empty)
                                                .Substring(0, 12));
            newAccount.AccountNumber = accountNumber;
            AccountRepo.Add(accountNumber, newAccount);
            return accountNumber;
        }

        public bool DeleteAccount(int accountNumber)
        {
           return AccountRepo.Remove(accountNumber);
        }

        public bool EditAccount(Accounts account)
        {
            AccountRepo[account.AccountNumber] = account;
            
            return true;
        }

        public Accounts GetAccountDetailsByAccountNumber(int accountNumber)
        {
            return AccountRepo[accountNumber];
        }
        public List<Accounts> GetAccountDetailsByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;
            else
                return new List<Accounts>(
               AccountRepo.Values.Where(account => account.UserName.CompareTo(userName) == 0)
               );
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (AccountRepo != null)
                        AccountRepo = null;
                }

               

                disposedValue = true;
            }
        }

       
        public void Dispose()
        {
            
            Dispose(true);
            
        }
        #endregion

    }
}
