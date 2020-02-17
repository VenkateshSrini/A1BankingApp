using A1.BankingApp.baseTypes;
using A1.BankingApp.Exceptions;
using A1.BankingApp.Repository;
using System;

namespace A.BankingApp
{
    public class SavingsAccount : Accounts
    {
        public override string TypeOfAccount => "SB";
     
        private double toAccountNumber;
        
        public override double ToAccount { get=> toAccountNumber;  }
        public const int MaxAmountPerday = 4000;
        public SavingsAccount(ILedgerRepo ledgerRepo, IAccountRepo accountRepo) : base(ledgerRepo, accountRepo)
        {

        }
        public double OpenAccount(string userName, double openingBalance)
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
            var totalAmount = ledgerRepo.GetAmountPerdayPeractivty(Activity.WITHDRAW);
            if ((totalAmount+amount) > MaxAmountPerday)
            {
                ValidationErrMsg = "Amount withdrawl failed: Max Amount per day exceeded ";
                throw new BankException(this);
            }
            else
            {
                if (base.Withdrawal(amount, this) != null)
                    return true;
                else
                {
                    ValidationErrMsg = "Amount withdrawl failed";
                    throw new BankException(this);
                }
            }
        }
        public override double GetRateOfInterest()
        {
            return 8.5;
        }

        public bool TransferAmount(double toAccount, double amountToTransfer)
        {
          
            toAccountNumber = toAccount;
         
            if (base.TransferAmount(amountToTransfer))
            {
                return true;
            }
            else
            {
                ValidationErrMsg = "In valid account number";
                throw new BankException(this);
            }

        }
        
    }
}
