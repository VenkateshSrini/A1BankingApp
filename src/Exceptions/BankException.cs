using A1.BankingApp.baseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.Exceptions
{
    public class BankException:Exception
    {
        public Accounts accounts { get; private set; }
        public BankException(Accounts accounts)
        {
            this.accounts = accounts
        }
        public override string Message => $"{accounts.ValidationErrMsg} \n {accounts.AccountNumber} {accounts.UserName}";

    }
}
