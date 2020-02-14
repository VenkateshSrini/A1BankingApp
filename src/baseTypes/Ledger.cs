using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.baseTypes
{
    public enum Activity
    {
        WITHDRAW=0,
        DEPOSIT=1,
        TRANSFER = 2
    }
    public enum TransactionType
    {
        CREDIT =0,
        DEBIT =1

    }
    public class Ledger
    {
        public DateTime ledgerEntryDT { get; set; }
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public string Description { get; set; }
        public Activity LedgerActivity { get; set; }
        public double TransactionAmount { get; set; }
        public TransactionType LedgerTransactionType { get; set; }
        

    }
}
