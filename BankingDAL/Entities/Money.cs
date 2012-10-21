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
    }
}
