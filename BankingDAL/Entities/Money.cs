using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Money : Entity
    {
        public virtual Currency Currency { get; set; }

        public double Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Value, Currency);
        }
    }
}
