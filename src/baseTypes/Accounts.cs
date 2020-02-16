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
        
        public virtual int AccountNumber { get; internal set; }
        public string UserName { get; internal set; }
        public double Balance { get; protected set; }
        public string ValidationErrMsg { get; set; }
        
        protected ILedgerRepo ledgerRepo;
        protected IAccountRepo accountRepo;
        public Accounts(ILedgerRepo ledgerRepo, IAccountRepo accountRepo)
        {
            this.ledgerRepo = ledgerRepo;
            this.accountRepo = accountRepo;
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
                return accountRepo.AddAccount(newAccount);
            }
            
        }
        protected virtual bool Close(Accounts account)
        {
            var accounts = accountRepo.GetAccountDetailsByAccountNumber(account.AccountNumber);
            
            if (accounts.Balance>=1)
            {
                account.ValidationErrMsg += " Balance amount is not zero";
                throw new BankException(account);
            }
            if (accountRepo.DeleteAccount(account.AccountNumber))
                return true;
            else
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);
            }



        }
        protected virtual Accounts EditAccount(Accounts account)
        {
            var accounts = accountRepo.GetAccountDetailsByAccountNumber(account.AccountNumber);
            if (accounts == null)
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);
            }
            else if(accountRepo.EditAccount(accounts))
            {
                return accounts;
            }
            else
            {
                return null;
            }
        }
        protected virtual Accounts Deposit(double depositAmount, Accounts account)
        {
            var accounts = accountRepo.GetAccountDetailsByAccountNumber(account.AccountNumber);
            if (accounts == null)
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);
            }
            else if (depositAmount >= 0)
            {
                accounts.Balance = accounts.Balance + depositAmount;
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
                return accounts;
            }
            return null;

        }
        protected Accounts Withdrawal(double withdrawAmount, Accounts account)
        {
            var accounts = accountRepo.GetAccountDetailsByAccountNumber(account.AccountNumber);
            if (accounts == null)
            {
                account.ValidationErrMsg += " account number does not exist";
                throw new BankException(account);
            }
            else if (withdrawAmount >= 0)
            {
                accounts.Balance = accounts.Balance - withdrawAmount;
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
                return accounts;
            }
            return null;
          
        }
        protected double CheckBalance(int accountNumber)
        {
            var accounts = accountRepo.GetAccountDetailsByAccountNumber(accountNumber);
            if (accounts == null)
            {
                accounts.ValidationErrMsg += " account number does not exist";
                throw new BankException(accounts);
            }
            else
                return accounts.Balance;
            
        }
        protected virtual Accounts GetAccountsDetails(int accountNumber)
        {
            return accountRepo.GetAccountDetailsByAccountNumber(accountNumber);
        }
        public virtual List<Accounts>GetAccountDetails(string userName)
        {
            return accountRepo.GetAccountDetailsByUserName(userName);
        }
        public bool TransferAmount(double amountToTransfer)
        {
            var fromAccount = accountRepo.GetAccountDetailsByAccountNumber(FromAccount);
            var toAccount = accountRepo.GetAccountDetailsByAccountNumber(ToAccount);
            if (fromAccount ==null)
            {
                ValidationErrMsg = "From account is not found";
                throw new BankException(fromAccount);
            }
            else if (toAccount == null)
            {
                ValidationErrMsg = "To account does not exist";
                throw new BankException(toAccount);

            }
            else
            {
                toAccount.Balance += amountToTransfer;
                fromAccount.Balance -= amountToTransfer;
                Ledger ledger = new Ledger
                {
                    Description = $"Amount{amountToTransfer} Deposited in Person",
                    FromAccount = FromAccount,
                    ToAccount = ToAccount,
                    LedgerActivity = Activity.TRANSFER,
                    ledgerEntryDT = DateTime.Today,
                    TransactionAmount = amountToTransfer,
                    LedgerTransactionType = TransactionType.DEBIT

                };
                ledgerRepo.AddLedgerEntry(ledger);

                return true;
            }
            
            
        }
    }
}
