using A1.BankingApp.contracts;
using A1.BankingApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace A1.BankingApp.baseTypes
{
    public abstract class Accounts : IROI, ITransaction
    {
        public abstract string TypeOfAccount { get; }
        public abstract int FromAccount { get;  }
        public abstract int ToAccount { get;  }

        public abstract double GetRateOfInterest();
        protected virtual bool TransferAmount(double amountToTransfer)
        {
            if ((FromAccount > 0) && (ToAccount>0) && AccountRepo.ContainsKey(FromAccount) 
                && AccountRepo.ContainsKey(ToAccount))
            {
                AccountRepo[ToAccount].Balance += amountToTransfer;
                AccountRepo[FromAccount].Balance -= amountToTransfer;
                return true;
            }
            return false;

        }
        public virtual int AccountNumber { get; protected set; }
        public string UserName { get; private set; }
        public double Balance { get; protected set; }
        public string ValidationErrMsg { get; set; }
        protected Dictionary<int, Accounts> AccountRepo = new Dictionary<int, Accounts>();
        protected virtual int OpenAccount(Accounts newAccount )
        {
            if (newAccount.Balance <1000)
            {
                newAccount.ValidationErrMsg = "Minim intial amout should be 1000";
                throw new BankException(newAccount);
            }
            else
            {
                var temp = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var accountNumber = int.Parse(Regex.Replace(temp, "[a-zA-Z]", string.Empty).Substring(0, 12));
                AccountRepo.Add(accountNumber, newAccount);
                return accountNumber;
            }
            
        }
        protected virtual bool Close(Accounts account)
        {
            
            if (AccountRepo.ContainsKey(account.AccountNumber))
            {
                if (AccountRepo[account.AccountNumber].Balance>=1)
                {
                    account.ValidationErrMsg += " Balance amount is not zero";
                    throw new BankException(account);
                }
                AccountRepo.Remove(account.AccountNumber);
                return true;
            }
            else
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);
            }
        }
        protected virtual Accounts EditAccount(Accounts account)
        {
            if (AccountRepo.ContainsKey(account.AccountNumber))
            {
                AccountRepo[account.AccountNumber].UserName = account.UserName;
                return AccountRepo[account.AccountNumber];
            }
            else
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);
            }
        }
        protected virtual Accounts Deposit(double depositAmount, Accounts account)
        {
            if (AccountRepo.ContainsKey(account.AccountNumber))
            {
                if (depositAmount >= 0)
                {
                    AccountRepo[account.AccountNumber].Balance = AccountRepo[account.AccountNumber].Balance + depositAmount;
                    return AccountRepo[account.AccountNumber];
                }
                return null;   
                
            }
            else
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);

            }
        }
        protected Accounts Withdrawal(double withdrawAmount, Accounts account)
        {
            if (AccountRepo.ContainsKey(account.AccountNumber))
            {
                if (withdrawAmount >= 0)
                {
                    AccountRepo[account.AccountNumber].Balance = 
                        AccountRepo[account.AccountNumber].Balance - withdrawAmount;
                    return AccountRepo[account.AccountNumber];
                }
                return null;

            }
            else
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);

            }
        }
        protected double CheckBalance(int accountNumber)
        {

            if (AccountRepo.ContainsKey(accountNumber))
            {
                
                return AccountRepo[accountNumber].Balance;

            }
            return -1;
        }
    }
}
