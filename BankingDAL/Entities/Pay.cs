﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Pay : Entity
    {
        public virtual Account From { get; set; }

        public virtual Account To { get; set; }

        public virtual Money Value { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Span { get; set; }
    }
}