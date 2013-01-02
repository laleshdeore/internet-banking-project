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

        public override string ToString()
        {
            return String.Format("{0} / {1} ({2})", First, Second, Type);
        }
    }
}
