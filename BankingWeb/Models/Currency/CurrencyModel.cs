using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models.Currency
{
    public class CurrencyModel
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Symbol { get; set; }

        public IList<CurrencyRate> Rates { get; set; } 

        public BankingDAL.Entities.Currency GetEntity()
        {
            return new BankingDAL.Entities.Currency
                       {
                           Name = Name,
                           ShortName = ShortName,
                           Symbol = Symbol,
                       };
        }

        public void SetEntity(BankingDAL.Entities.Currency currency)
        {
            Name = currency.Name;
            ShortName = currency.ShortName;
            Symbol = currency.Symbol;
        }
    }
}