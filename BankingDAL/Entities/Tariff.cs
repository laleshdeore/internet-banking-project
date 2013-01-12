using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Tariff: Entity
    {
        public virtual Money MonthlyPay { get; set; }
    }
}
