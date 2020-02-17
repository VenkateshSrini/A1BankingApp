using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.contracts
{
    public interface ITransaction
    {
        double FromAccount { get;  }
        double ToAccount { get;  }
         bool TransferAmount(double amountToTransfer);

    }
}
