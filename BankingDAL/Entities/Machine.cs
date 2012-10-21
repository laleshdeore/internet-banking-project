using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Machine : Entity
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
