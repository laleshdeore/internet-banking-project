using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Payment : Entity
    {
        public virtual Account From { get; set; }

        public virtual Account To { get; set; }

        public virtual Money Value { get; set; }

        public DateTime Date { get; set; }

        public string ServiceIdentifier { get; set; }

        public long Span { get; set; }

        public PaymentState State
        {
            get { return (PaymentState)StateInt; }
            set { StateInt = (int)value; }
        }

        public int StateInt { get; set; }
    }
}
