using A.BankingApp.Functional.Test.Fixtures;
using A1.BankingApp;
using A1.BankingApp.Exceptions;
using A1.BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace A.BankingApp.Functional.Test
{
    [Collection("CurrentAccount")]
    public class CurrentAccountsTest:IClassFixture<RespositoryFixture>
    {
        private readonly IAccountRepo AccountRepo;
        private readonly ILedgerRepo LedgerRepo;
        public CurrentAccountsTest(RespositoryFixture repofixture)
        {
            AccountRepo = repofixture.AccountRepo;
            LedgerRepo = repofixture.LedgerRepo;
        }
        [Theory]
        [InlineData("a", 1001)]
        [InlineData(" ", 1001)]
        [InlineData("b", 10)]
        public void OpenDepositWithDrawCloseAccountTest(string userName, double minimumBalance)
        {
            //Arrange
            CurrentAccount currentAccount = new CurrentAccount(LedgerRepo, AccountRepo);
            if ((!string.IsNullOrWhiteSpace(userName)) && (minimumBalance > 1000))
            {   //Act
                var accountNumber = currentAccount.OpenAccount(userName, minimumBalance);
                var accountNumberLength = accountNumber.ToString().Length;
                //Assert
                Assert.Equal(12, accountNumberLength);

                currentAccount.DepositAmount(7000);
                var withdrawStatus = currentAccount.WithdrawlAmount(2001);
                
                Assert.True(withdrawStatus);
                Assert.True(currentAccount.CloseAccount());
         
            }
            else if (string.IsNullOrWhiteSpace(userName))
            {

                Assert.Throws<BankException>(() =>
                    currentAccount.OpenAccount("", minimumBalance)
                    );
            }
            
        }
        [Theory]
        [InlineData("c", 1001, "d")]
        public void OpenEditGetDetailsCheckBalance(string userName, double balance, string newUserName)
        {
            CurrentAccount currentAccount = new CurrentAccount(LedgerRepo, AccountRepo);
            var accountNumber = currentAccount.OpenAccount(userName, balance);
            Assert.True(currentAccount.EditAccountDetails(newUserName));
            var modeifiedAccount = currentAccount.GetAccountDetails(newUserName);
            Assert.Equal(newUserName, modeifiedAccount[0].UserName);
            Assert.Equal(accountNumber, modeifiedAccount[0].AccountNumber);
            Assert.Equal(balance, modeifiedAccount[0].Balance);
        }
        [Theory]
        [InlineData("a", 7001, "b", 2001)]
        public void TransferTypeofACMaxTransPerdayTest(string userName, double bal1, string userName1, double bal2)
        {
            CurrentAccount currentAccount = new CurrentAccount(LedgerRepo, AccountRepo);
            CurrentAccount currentAccount1 = new CurrentAccount(LedgerRepo, AccountRepo);
            var fromAccountNumber = currentAccount.OpenAccount(userName, bal1);
            var toAccountNumber = currentAccount1.OpenAccount(userName1, bal2);
            Assert.True(currentAccount.TransferAmount(toAccountNumber, 100));
            Assert.Equal("CB", currentAccount.TypeOfAccount);
            Assert.Equal(3000, CurrentAccount.MinimumBalance);
            Assert.Equal(5, currentAccount.GetRateOfInterest());
        }
        [Theory]
        [InlineData("c", 7001, "c", 7000)]
        public void MinimumBalancePerday(string userName, double bal1, string userName1, double bal2)
        {
            CurrentAccount currentAccount = new CurrentAccount(LedgerRepo, AccountRepo);
            CurrentAccount currentAccount1 = new CurrentAccount(LedgerRepo, AccountRepo);
            var fromAccountNumber = currentAccount.OpenAccount(userName, bal1);
            var toAccountNumber = currentAccount1.OpenAccount(userName1, bal2);
            Assert.Throws<BankException>(() => currentAccount.TransferAmount(toAccountNumber, bal2));
        }

    }
}
