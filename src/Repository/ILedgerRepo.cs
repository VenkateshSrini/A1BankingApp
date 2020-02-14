using A1.BankingApp.baseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.Repository
{
    public interface ILedgerRepo
    {
        bool AddLedgerEntry(Ledger ledger);
        int GetLedgerEntryCountForTodayPerActivity(Activity activity);
        double GetAmountPerdayPeractivty(Activity activity);
        
    }
}
