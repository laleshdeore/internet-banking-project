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

        public Currency GetCurrencyByName(string name)
        {
            return Database.Currencies.SingleOrDefault(currency => currency.Name == name);
        }

        public CurrencyRate GetCurrencyRateById(long id)
        {
            return Database.Rates.SingleOrDefault(rate => rate.Id == id);
        }

        public IList<CurrencyRate> GetCurrencyRates(Currency currency)
        {
            return Database.Rates.Where(rate => rate.First.Currency.Id == currency.Id || rate.Second.Currency.Id == currency.Id).ToList();
        }

        public IList<CurrencyRate> GetCurrencyRates(Currency first, Currency second)
        {
            return Database.Rates.Where(rate => rate.Currencies.Select(c => c.Id).Contains(first.Id) && rate.Currencies.Select(c => c.Id).Contains(second.Id)).ToList();
        }

        public IList<Money> GetPossibleMoneys()
        {
            var moneys = new List<Money>();

            foreach (var currency in Database.Currencies)
            {
                moneys.Add(new Money {Currency = currency});
            }
            return moneys;
        }

        public Money Convert(Money money, Currency currency)
        {
            var rates = GetCurrencyRates(money.Currency, currency);
            var buyRate = rates.SingleOrDefault(rate => rate.Type == CurrencyRateType.Buy);
            var sellRate = rates.SingleOrDefault(rate => rate.Type == CurrencyRateType.Sell);

            return money;
        }

        public long Add(Currency currency)
        {
            currency = Database.Currencies.Add(currency);

            Database.Bank.Balance.Add(new Money { Currency = currency });
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

            SaveAllChanges();
            return currency.Id;
        }

        public void AddOrUpdate(CurrencyRate rate)
        {
            if (rate.Id == 0)
            {
                Database.Rates.Add(rate);
                SaveAllChanges();
            }
            else
            {
                Update(GetCurrencyRateById(rate.Id), rate);
            }
        }
    }
}
