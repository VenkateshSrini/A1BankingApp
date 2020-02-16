using A.BankingApp.Functional.Test.Fixtures;
using A1.BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace A.BankingApp.Functional.Test
{
    public class CurrentSavingAccounts
    {
        private readonly IAccountRepo AccountRepo;
        private readonly ILedgerRepo LedgerRepo;
        public CurrentSavingAccounts(RespositoryFixture repofixture)
        {
            AccountRepo = repofixture.AccountRepo;
            LedgerRepo = repofixture.LedgerRepo;
        }
    }
}
