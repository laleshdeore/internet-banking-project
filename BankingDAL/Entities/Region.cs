using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Region : Entity
    {
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }

        public virtual List<Service> Services { get; set; } 
    }
}
