using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;

namespace BankingWeb.Models
{
    public class CurrencyRatesModel
    {
        public Currency Currency { get; set; }

        public IList<Currency> Currencies { get; set; }

        public IList<CurrencyRate> Rates { get; set; } 
    }
}