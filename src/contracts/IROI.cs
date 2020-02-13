using System;
using System.Collections.Generic;
using System.Text;

namespace A1.BankingApp.contracts
{
    interface IROI
    {
        string TypeOfAccount { get; }
        double GetRateOfInterest();
    }
}
