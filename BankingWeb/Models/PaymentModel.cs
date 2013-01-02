using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models
{
    public class PaymentModel
    {
        public PaymentModel()
        {
            Accounts = new List<AccountModel>();
            Amount = new List<MoneyModel>();
        }

        public AccountModel From { get; set; }

        public AccountModel To { get; set; }

        public MoneyModel Value { get; set; }

        public DateTime Date { get; set; }

        public List<AccountModel> Accounts { get; set; }

        public List<MoneyModel> Amount { get; set; }

        public string PersonalCode { get; set; }

        public long? Span { get; set; }

        public Payment GetEntity(IUserRepository userRepository, ICurrencyRepository currencyRepository)
        {
            return new Payment
            {
                Date = DateTime.Now,
                From = From.GetEntity(userRepository, currencyRepository),
                To = To.GetEntity(userRepository, currencyRepository),
                Span = Span.GetValueOrDefault(0),
                State = PaymentState.Pending,
                Value = Value.GetEntity(currencyRepository)
            };
        }
    }
}