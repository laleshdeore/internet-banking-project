using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class CurrencyRate : Entity
    {
        public virtual Money First { get; set; }

        public virtual Money Second { get; set; }

        public CurrencyRateType Type
        {
            get { return (CurrencyRateType) TypeInt; }
            set { TypeInt = (int) value; }
        }

        public int TypeInt { get; set; }

        public override string ToString()
        {
            return String.Format("{0} / {1} ({2})", First, Second, Type);
        }
    }
}
