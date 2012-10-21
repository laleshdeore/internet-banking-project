using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Message : Entity
    {
        public virtual User Author { get; set; }

        public virtual User Recipient { get; set; }

        public DateTime Date { get; set; }

        public bool IsReceived { get; set; }
    }
}
