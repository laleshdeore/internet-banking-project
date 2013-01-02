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

        public virtual List<Machine> Machines { get; set; }

        public virtual List<Money> Balance { get; set; } 

        public Bank()
        {
            Users = new List<User>();
            Accounts = new List<Account>();
            Balance = new List<Money>();
        }
    }
}
