using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;

namespace BankingWeb.Models
{
    public class RegionModel
    {
        public string Name { get; set; }

        public IList<Region> All { get; set; }
 
        public RegionModel()
        {
            All = new List<Region>();
        }

        public Region GetEntity()
        {
            return new Region {Name = Name};
        }
    }
}