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
            Currencies = new List<Currency>();
        }

        public AccountModel From { get; set; }

        public AccountModel To { get; set; }

        public MoneyModel Value { get; set; }

        public DateTime Date { get; set; }

        public IList<AccountModel> Accounts { get; set; }

        public IList<Currency> Currencies { get; set; }

        public Service Service { get; set; }

        public string ServiceIdentifier { get; set; }

        public string Description { get; set; }

        public string PersonalCode { get; set; }

        public string ExpirationDate { get; set; }

        public string IsAutomatic { get; set; }

        public Payment GetEntity(ICurrencyRepository currencyRepository, IAccountRepository accountRepository)
        {
            return new Payment
            {
                Date = DateTime.Now,
                From = accountRepository.GetAccountByNumber(From.Number),
                To = accountRepository.GetAccountByNumber(To.Number),
                ServiceIdentifier = ServiceIdentifier,
                IsAutomatic = IsAutomatic == "on" || (IsAutomatic != null && bool.Parse(IsAutomatic)),
                State = PaymentState.Pending,
                Description = Description,
                Value = Value.GetEntity(currencyRepository)
            };
        }
    }
}