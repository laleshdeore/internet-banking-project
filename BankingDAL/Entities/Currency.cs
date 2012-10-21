using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Currency : Entity
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Symbol { get; set; }
    }
}
