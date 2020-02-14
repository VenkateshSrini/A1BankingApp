using A1.BankingApp.contracts;
using A1.BankingApp.Exceptions;
using A1.BankingApp.Repository;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace A1.BankingApp.baseTypes
{
    public abstract class Accounts : IROI, ITransaction
    {
        public abstract string TypeOfAccount { get; }
        public virtual int FromAccount { get=> AccountNumber;  }
        public abstract int ToAccount { get;  }

        public abstract double GetRateOfInterest();
        
        public virtual int AccountNumber { get; protected set; }
        public string UserName { get; protected set; }
        public double Balance { get; protected set; }
        public string ValidationErrMsg { get; set; }
        protected Dictionary<int, Accounts> AccountRepo = new Dictionary<int, Accounts>();
        protected ILedgerRepo ledgerRepo;
        public Accounts(ILedgerRepo ledgerRepo)
        {
            this.ledgerRepo = ledgerRepo;
        }

        protected virtual int OpenAccount(Accounts newAccount )
        {
            if (newAccount.Balance <1000)
            {
                newAccount.ValidationErrMsg = "Minimum intial amout should be 1000";
                throw new BankException(newAccount);
            }
            if (string.IsNullOrWhiteSpace(newAccount.UserName))
            {
                newAccount.ValidationErrMsg = "User name is blank";
                throw new BankException(newAccount);
            }
            else
            {
                var temp = Guid.NewGuid().ToString().Replace("-", string.Empty);
                var accountNumber = int.Parse(Regex.Replace(temp, "[a-zA-Z]", string.Empty).Substring(0, 12));
                newAccount.AccountNumber = accountNumber;
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
                    Ledger ledger = new Ledger
                    {
                        Description = $"Amount{depositAmount} Deposited in Person",
                        FromAccount = account.AccountNumber,
                        LedgerActivity = Activity.DEPOSIT,
                        ledgerEntryDT = DateTime.Today,
                        TransactionAmount = depositAmount,
                        LedgerTransactionType = TransactionType.CREDIT

                    };
                    ledgerRepo.AddLedgerEntry(ledger);
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
                    Ledger ledger = new Ledger
                    {
                        Description = $"Amount{withdrawAmount} Deposited in Person",
                        FromAccount = account.AccountNumber,
                        LedgerActivity = Activity.WITHDRAW,
                        ledgerEntryDT = DateTime.Today,
                        TransactionAmount = withdrawAmount,
                        LedgerTransactionType = TransactionType.DEBIT

                    };
                    ledgerRepo.AddLedgerEntry(ledger);
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
        protected virtual Accounts GetAccountsDetails(int accountNumber)
        {
            if (AccountRepo.ContainsKey(accountNumber))
                return AccountRepo[accountNumber];
            else
                return null;
        }
        public virtual List<Accounts>GetAccountDetails(string userName)
        {
            return new List<Accounts>(
                AccountRepo.Values.Where(account => account.UserName.CompareTo(userName) == 0)
                );
        }
        public bool TransferAmount(double amountToTransfer)
        {
            if ((FromAccount > 0) && (ToAccount > 0) && AccountRepo.ContainsKey(FromAccount)
                && AccountRepo.ContainsKey(ToAccount))
            {
                AccountRepo[ToAccount].Balance += amountToTransfer;
                AccountRepo[FromAccount].Balance -= amountToTransfer;
                Ledger ledger = new Ledger
                {
                    Description = $"Amount{amountToTransfer} Deposited in Person",
                    FromAccount = FromAccount,
                    ToAccount= ToAccount,
                    LedgerActivity = Activity.TRANSFER,
                    ledgerEntryDT = DateTime.Today,
                    TransactionAmount = amountToTransfer,
                    LedgerTransactionType = TransactionType.DEBIT

                };
                ledgerRepo.AddLedgerEntry(ledger);

                return true;
            }
            return false;
        }
    }
}
