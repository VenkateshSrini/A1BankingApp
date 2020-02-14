using A1.BankingApp.baseTypes;
using A1.BankingApp.Exceptions;
using System;

namespace A.BankingApp
{
    public class SavingsAccount : Accounts
    {
        public override string TypeOfAccount => "SB";
        private int fromAccountNuber;
        private int toAccountNumber;
        public override int FromAccount { get=> fromAccountNuber;  }
        public override int ToAccount { get=> toAccountNumber;  }
        public int MaxAmountPerDay{get=>40000;}
        public override double GetRateOfInterest()
        {
            throw new NotImplementedException();
        }

        public bool TransferAmount(int fromAccount, int toAccount, double amountToTransfer)
        {
            fromAccountNuber = fromAccount;
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
