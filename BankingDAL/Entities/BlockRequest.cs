using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class BlockRequest : Entity
    {
        public virtual Account Account { get; set; }

        public virtual User Author { get; set; }

        public string Reason { get; set; }
    }
}
