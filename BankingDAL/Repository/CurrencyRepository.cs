using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class CurrencyRepository : DatabaseRepository, ICurrencyRepository
    {
        public CurrencyRepository(DatabaseContext database)
            : base(database)
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

        public Currency GetCurrencyById(long id)
        {
            return Database.Currencies.SingleOrDefault(currency => currency.Id == id);
        }

        public CurrencyRate GetCurrencyRateById(long id)
        {
            return Database.Rates.SingleOrDefault(rate => rate.Id == id);
        }

        public IList<CurrencyRate> GetCurrencyRates(Currency currency)
        {
            return Database.Rates.Where(rate => rate.First.Currency.Id == currency.Id || rate.Second.Currency.Id == currency.Id).ToList();
        }

        public long Add(Currency currency)
        {
            currency = Database.Currencies.Add(currency);

            Database.SaveChanges();

            foreach (var otherCurrency in Database.Currencies.Where(otherCurrency => currency.Id != otherCurrency.Id).ToList())
            {
                AddOrUpdate(new CurrencyRate
                                {
                                    First = new Money { Currency = currency },
                                    Second = new Money { Currency = otherCurrency },
                                    Type = CurrencyRateType.Buy
                                });
                AddOrUpdate(new CurrencyRate
                                {
                                    First = new Money { Currency = currency },
                                    Second = new Money { Currency = otherCurrency },
                                    Type = CurrencyRateType.Sell
                                });
            }

            return currency.Id;
        }

        public void AddOrUpdate(CurrencyRate rate)
        {
            if (rate.Id == 0)
            {
                Database.Rates.Add(rate);
            }
            else
            {
                var updateRate = GetCurrencyRateById(rate.Id);

                if (updateRate != null)
                {
                    Database.Entry(updateRate).CurrentValues.SetValues(rate);
                }
            }

            while (Database.SaveChanges() != 0) { }
        }
    }
}
