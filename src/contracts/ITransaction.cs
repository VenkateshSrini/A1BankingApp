using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.contracts
{
    public interface ITransaction
    {
        int FromAccount { get; set; }
        int ToAccount { get; set; }
         bool TransferAmount(double amountToTransfer);

    }
}
