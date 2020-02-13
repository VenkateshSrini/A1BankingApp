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
        public abstract int FromAccount { get; set; }
        public abstract int ToAccount { get; set; }

        public abstract double GetRateOfInterest();
        public abstract bool TransferAmount(double amountToTransfer);
        public virtual int AccountNumber { get; protected set; }
        public string UserName { get; private set; }
        public double Balance { get; protected set; }
        public string ValidationErrMsg { get; set; }
        protected Dictionary<int, Accounts> AccountRepo = new Dictionary<int, Accounts>();
        protected virtual int OpenAccount(Func<Tuple<bool,string>>validation,Accounts newAccount )
        {
            var validationResult = validation();
         
            if (validationResult.Item1)
            {
                var temp = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var accountNumber = int.Parse(Regex.Replace(temp, "[a-zA-Z]", string.Empty).Substring(0, 12));
                AccountRepo.Add(accountNumber, newAccount);
                return accountNumber;

            }
            else
            {
                newAccount.ValidationErrMsg = validationResult.Item2;
                throw new BankException(newAccount);
                
            }
            
        }
        protected virtual bool Close(Accounts account)
        {
            if (AccountRepo.ContainsKey(account.AccountNumber))
            {
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
