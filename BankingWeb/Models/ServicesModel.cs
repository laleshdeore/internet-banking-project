using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models
{
    public class ServicesModel
    {
        public IList<Service> Services { get; set; }
        public Page Page { get; set; }

        public ServicesModel()
        {
            Services = new List<Service>();
            Page = new Page();
        }
    }
}