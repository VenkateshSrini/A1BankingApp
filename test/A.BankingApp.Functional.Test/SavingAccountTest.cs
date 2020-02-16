using A.BankingApp.Functional.Test.Fixtures;
using A1.BankingApp.Repository;
using System;
using Xunit;

namespace A.BankingApp.Functional.Test
{
    public class SavingAccountTest:IClassFixture<RespositoryFixture>
    {
        private readonly IAccountRepo AccountRepo;
        private readonly ILedgerRepo LedgerRepo;
        public SavingAccountTest(RespositoryFixture repofixture)
        {
            AccountRepo = repofixture.AccountRepo;
            LedgerRepo = repofixture.LedgerRepo;
        }
        [Fact]
        public void Test1()
        {

        }
    }
}
