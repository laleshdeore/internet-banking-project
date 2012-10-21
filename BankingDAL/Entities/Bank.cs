using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Bank : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<User> Users { get; set; }

        public virtual List<Account> Accounts { get; set; }

        public virtual List<Machine> Machinges { get; set; }

        public List<Money> Balance { get; set; } 
    }
}
