using System;
using System.Collections.Generic;

namespace BankingDAL.Entities
{
    public class CurrencyRate : Entity
    {
        public virtual Money First { get; set; }

        public virtual Money Second { get; set; }

        public IList<Currency> Currencies
        {
            get
            {
                var currencies = new List<Currency>();

                if (First != null)
                {
                    currencies.Add(First.Currency);
                }
                if (Second != null)
                {
                    currencies.Add(Second.Currency);
                }

                return currencies;
            }
        }

        public CurrencyRateType Type
        {
            get { return (CurrencyRateType)TypeInt; }
            set { TypeInt = (int)value; }
        }

        public int TypeInt { get; set; }

        public double Rate(Currency currency)
        {
            var rate = 0.0;

            if (First.Currency == currency)
            {
                rate = First.Value / Second.Value;
            }
            else if (Second.Currency == currency)
            {
                rate = Second.Value / First.Value;
            }
            return rate;
        }

        public override string ToString()
        {
            return String.Format("{0} / {1} ({2})", First, Second, Type);
        }
    }
}
