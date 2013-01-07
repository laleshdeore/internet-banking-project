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

        Currency GetCurrencyById(long id);

        Currency GetCurrencyByName(string name);

        CurrencyRate GetCurrencyRateById(long id);

        IList<CurrencyRate> GetCurrencyRates(Currency currency);

        IList<CurrencyRate> GetCurrencyRates(Currency first, Currency second);

        IList<Money> GetPossibleMoneys();

        Money Convert(Money money, Currency currency);

        long Add(Currency currency);

        void AddOrUpdate(CurrencyRate rate);

        void Delete(Currency currency);

        void Delete(CurrencyRate rate);
    }
}
