using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class CurrencyRepository: DatabaseRepository, ICurrencyRepository
    {
        public CurrencyRepository(DatabaseContext database) : base(database)
        {
        }

        public IList<Currency> GetCurrencies()
        {
            return Database.Currencies.ToList();
        }

        public IList<CurrencyRate> GetCurrencyRates()
        {
            return Database.Rates.ToList();
        }
    }
}
