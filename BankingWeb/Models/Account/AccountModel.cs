using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankingWeb.Models.Account
{
    public class AccountModel
    {
        public string Number { get; set; }

        public string Owner { get; set; }

        public IList<MoneyModel> Balance { get; set; } 
    }

    public class MoneyModel
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public double? Value { get; set; }
    }
}