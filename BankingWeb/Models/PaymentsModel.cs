using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models
{
    public class PaymentsModel
    {
        public IList<Payment> Payments { get; set; }
        public Page Page { get; set; }

        public PaymentsModel()
        {
            Payments = new List<Payment>();
            Page = new Page();
        }
    }
}