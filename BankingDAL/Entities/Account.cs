using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Account : Entity
    {
        public string Number { get; set; }

        public virtual List<Money> Balance { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsActive { get; set; }
    }
}
