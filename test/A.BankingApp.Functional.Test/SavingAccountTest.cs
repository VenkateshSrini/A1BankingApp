using A.BankingApp.Functional.Test.Fixtures;
using A1.BankingApp.Exceptions;
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
        [Theory]
        [InlineData("a",1001)]
        [InlineData(" ", 1001)]
        [InlineData("b", 10)]
        public void OpenDepositWithDrawCloseAccountTest(string userName, double minimumBalance)
        {
            //Arrange
            SavingsAccount savingsAccount = new SavingsAccount(LedgerRepo, AccountRepo);
            if ((!string.IsNullOrWhiteSpace(userName)) && (minimumBalance > 1000))
            {   //Act
                var accountNumber = savingsAccount.OpenAccount(userName, minimumBalance);
                var accountNumberLength = accountNumber.ToString().Length;
                //Assert
                Assert.Equal(12, accountNumberLength);
                
                savingsAccount.DepositAmount(1000);
                var withdrawStatus = savingsAccount.WithdrawlAmount(2001);
                Assert.True(withdrawStatus);
                Assert.True(savingsAccount.CloseAccount());
            }
            else if (string.IsNullOrWhiteSpace(userName))
            {

                Assert.Throws<BankException>(() =>
                    savingsAccount.OpenAccount("", minimumBalance)
                    );
            }
            else if (minimumBalance <=999)
            {
                Assert.Throws<BankException>(() =>
                    savingsAccount.OpenAccount(userName, minimumBalance)
                    );
            }
        }

        [Theory]
        [InlineData("a", 1001,"b")]
        public void OpenEditGetDetailsCheckBalance(string userName, double balance, string newUserName)
        {
            SavingsAccount savingsAccount = new SavingsAccount(LedgerRepo, AccountRepo);
            var accountNumber = savingsAccount.OpenAccount(userName, balance);
            Assert.True(savingsAccount.EditAccountDetails(newUserName));
            var modeifiedAccount = savingsAccount.GetAccountDetails(newUserName);
            Assert.Equal(newUserName, modeifiedAccount[0].UserName);
            Assert.Equal(accountNumber, modeifiedAccount[0].AccountNumber);
            Assert.Equal(balance, modeifiedAccount[0].Balance);
        }
        [Theory]
        [InlineData ("a",1001,"b", 2001)]
        public void TransferTypeofACMaxTransPerdayTest(string userName, double bal1, string userName1, double bal2)
        {
            SavingsAccount savingsAccount = new SavingsAccount(LedgerRepo, AccountRepo);
            SavingsAccount savingsAccount1 = new SavingsAccount(LedgerRepo, AccountRepo);
            var fromAccountNumber = savingsAccount.OpenAccount(userName, bal1);
            var toAccountNumber = savingsAccount1.OpenAccount(userName1, bal2);
            Assert.True(savingsAccount.TransferAmount(toAccountNumber, 100));
            Assert.Equal("SB", savingsAccount.TypeOfAccount);
            Assert.Equal(40000, SavingsAccount.MaxAmountPerday);

        }
    }
}
