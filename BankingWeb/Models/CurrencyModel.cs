using System.Collections.Generic;
using BankingDAL.Entities;

namespace BankingWeb.Models
{
    public class CurrencyModel
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Symbol { get; set; }

        public IList<CurrencyRate> Rates { get; set; }

        public Currency GetEntity()
        {
            return new Currency
            {
                Name = Name,
                ShortName = ShortName,
                Symbol = Symbol,
            };
        }

        public void SetEntity(Currency currency)
        {
            Name = currency.Name;
            ShortName = currency.ShortName;
            Symbol = currency.Symbol;
        }
    }
}