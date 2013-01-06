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

        public string Owner { get; set; }

        public string PersonalCode { get; set; }

        public string ExpirationDate { get; set; }

        public bool IsAutomatic { get; set; }

        public Payment GetEntity(ICurrencyRepository currencyRepository, IAccountRepository accountRepository, IUserRepository userRepository)
        {
            var toAccount = (To != null && To.Number != null) ? accountRepository.GetAccountByNumber(To.Number) : userRepository.GetUserByUsername(Owner).Accounts.First();

            return new Payment
            {
                Date = DateTime.Now,
                From = accountRepository.GetAccountByNumber(From.Number),
                To = toAccount,
                ServiceIdentifier = ServiceIdentifier,
                IsAutomatic = IsAutomatic,
                State = PaymentState.Pending,
                Value = Value.GetEntity(currencyRepository)
            };
        }
    }
}