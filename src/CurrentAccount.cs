using A1.BankingApp.baseTypes;
using A1.BankingApp.Exceptions;
using A1.BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp
{
    public class CurrentAccount : Accounts
    {

        public override string TypeOfAccount => "CB";
        private int toAccountNumber;
        public override int ToAccount { get => toAccountNumber; }
        public const int MinimumBalance = 3000;
        public CurrentAccount(ILedgerRepo ledgerRepo) : base(ledgerRepo)
        {

        }
        public override double GetRateOfInterest()
        {
            return 5;
        }
        public int OpenAccount(string userName, double openingBalance)
        {
            this.Balance = openingBalance;
            this.UserName = userName;
            return base.OpenAccount(this);
        }
        public bool CloseAccount()
        {
            return base.Close(this);
        }
        public bool EditAccountDetails(string userName)
        {
            this.UserName = userName;
            if (base.EditAccount(this) != null)
                return true;
            else
            {
                ValidationErrMsg = "User details modification failed";
                throw new BankException(this);
            }

        }
        public bool DepositAmount(double amount)
        {
            if (base.Deposit(amount, this) != null)
                return true;
            else
            {
                ValidationErrMsg = "Amount Deposition failed";
                throw new BankException(this);
            }
        }
        public bool WithdrawlAmount(double amount)
        {
            if (this.Balance - amount <= 3000)
            {
                ValidationErrMsg = "Withdrawl failed as mimum balance is forefeited.";
                throw new BankException(this);
            }
            return (base.Withdrawal(amount, this) != null) ? true : throw new BankException(this);
            

        }
        public bool TransferAmount(int toAccount, double amountToTransfer)
        {

            toAccountNumber = toAccount;
            if (this.Balance - amountToTransfer <= 3000)
            {
                ValidationErrMsg = "Withdrawl failed as mimum balance is forefeited.";
                throw new BankException(this);
            }

            if (base.TransferAmount(amountToTransfer))
                return true;
            else
            {
                ValidationErrMsg = "In valid account number";
                throw new BankException(this);
            }

        }
    }
}
