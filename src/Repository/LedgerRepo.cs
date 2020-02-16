using A1.BankingApp.baseTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace A1.BankingApp.Repository
{
    public class LedgerRepository : ILedgerRepo, IDisposable
    {
        private  List<Ledger> ledgerEntry;
       public LedgerRepository()
        {
           ledgerEntry = new List<Ledger>();
        }

        public bool AddLedgerEntry(Ledger ledger)
        {
            ledgerEntry.Add(ledger);
            return true;
        }
        public int GetLedgerEntryCountForTodayPerActivity(Activity activity)
        {
            return ledgerEntry.Count<Ledger>(ledger =>
            ledger.LedgerActivity == activity && ledger.ledgerEntryDT==DateTime.Today);
        }
        public double GetAmountPerdayPeractivty(Activity activity)
        {
            return ledgerEntry.Sum(ledger => ((ledger.LedgerActivity == activity) &&
                                            (ledger.ledgerEntryDT == DateTime.Today)
                                            ) ? ledger.TransactionAmount : 0);
            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (ledgerEntry != null)
                        ledgerEntry = null;
                }

                

                disposedValue = true;
            }
        }

      
        
        public void Dispose()
        {
            
            Dispose(true);
            
        }
        #endregion
    }
}
