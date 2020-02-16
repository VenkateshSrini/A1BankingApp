using A1.BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace A.BankingApp.Functional.Test.Fixtures
{
    public class RespositoryFixture:IDisposable
    {
        public IAccountRepo AccountRepo { get; private set; }
        public ILedgerRepo LedgerRepo { get; private set; }
        public RespositoryFixture()
        {
            AccountRepo = new AccountRepository();
            LedgerRepo = new LedgerRepository();
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
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
