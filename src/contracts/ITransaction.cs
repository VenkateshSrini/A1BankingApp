using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.contracts
{
    public interface ITransaction
    {
        int FromAccount { get;  }
        int ToAccount { get;  }
         bool TransferAmount(double amountToTransfer);

    }
}
