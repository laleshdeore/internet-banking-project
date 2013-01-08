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
                moneys.Add(new Money { Currency = currency });
            }
            return moneys;
        }

        public Money Convert(Money money, Currency currency)
        {
            var rates = GetCurrencyRates(money.Currency, currency);
            var sellRate = rates.SingleOrDefault(rate => rate.Type == CurrencyRateType.Sell);
            var result = new Money { Currency = currency };

            if (money.Currency.Id == currency.Id)
            {
                return money;
            }

            if (money.Currency.Id == sellRate.First.Currency.Id)
            {
                result.Value = money.Value / sellRate.First.Value * sellRate.Second.Value;
            }
            else
            {
                result.Value = money.Value / sellRate.Second.Value * sellRate.First.Value;
            }

            return result;
        }

        public IList<Money> GetMoneys(Currency currency)
        {
            return Database.Moneys.Where(money => money.Currency.Id == currency.Id).ToList();
        }

        public long Add(Currency currency)
        {
            Validate(currency);
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

        public void Delete(Currency currency)
        {
            var rates = GetCurrencyRates(currency);
            var rateMoneys = rates.Select(r => r.First).Union(rates.Select(r => r.Second));
            var moneys = GetMoneys(currency);

            if (moneys.Any(money => !rateMoneys.Contains(money) && money.Value != 0))
            {
                throw new Exception("Bank has money with this currency");
            }
            foreach (var rate in GetCurrencyRates(currency))
            {
                Delete(rate);
            }
            foreach (var money in GetMoneys(currency))
            {
                Delete(money);
            }
            Database.Currencies.Remove(currency);
            SaveAllChanges();
        }

        public void Delete(CurrencyRate rate)
        {
            Delete(rate.First);
            Delete(rate.Second);
            Database.Rates.Remove(rate);
            SaveAllChanges();
        }

        public void Delete(Money money)
        {
            Database.Moneys.Remove(money);
            SaveAllChanges();
        }

        protected void Validate(Currency currency)
        {
            if (Database.Currencies.Any(c => c.Name.Equals(currency.Name, StringComparison)))
            {
                throw new Exception("Currency name must be unique");
            }
            if (Database.Currencies.Any(c => c.ShortName.Equals(currency.ShortName, StringComparison)))
            {
                throw new Exception("Currency short name must be unique");
            }
            if (Database.Currencies.Any(c => c.Symbol.Equals(currency.Symbol, StringComparison)))
            {
                throw new Exception("Currency symbol must be unique");
            }
        }
    }
}
