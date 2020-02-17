using A.BankingApp.Functional.Test.Fixtures;
using A1.BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace A.BankingApp.Functional.Test
{
    public class CurrentAccountsTest
    {
        private readonly IAccountRepo AccountRepo;
        private readonly ILedgerRepo LedgerRepo;
        public CurrentAccountsTest(RespositoryFixture repofixture)
        {
            AccountRepo = repofixture.AccountRepo;
            LedgerRepo = repofixture.LedgerRepo;
        }
    }
}
