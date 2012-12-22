using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface ICurrencyRepository
    {
        IList<Currency> GetCurrencies();

        IList<CurrencyRate> GetCurrencyRates();
    }
}
