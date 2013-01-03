using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Controllers;

namespace BankingWeb.Models
{
    public class AccountModel
    {
        public string Number { get; set; }

        public string Owner { get; set; }

        public string ExpirationDate { get; set; }

        public bool? IsActive { get; set; }

        public IList<MoneyModel> Balance { get; set; }

        public State State
        {
            get
            {
                var state = State.Ok;

                if (IsActive.GetValueOrDefault() && Balance.Sum(money => money.Value.GetValueOrDefault(0)) < BaseController.MoneyBarrier)
                {
                    state = State.Low;
                }
                else if (!IsActive.GetValueOrDefault())
                {
                    state = State.Blocked;
                }
                return state;
            }
        }

        public AccountModel():this(null)
        {
        }

        public AccountModel(Account entity)
        {
            Balance = new List<MoneyModel>();
            IsActive = true;
            SetEntity(entity);
        }

        public Account GetEntity(IUserRepository userRepository, ICurrencyRepository currencyRepository)
        {
            return new Account
            {
                ExpirationDate = DateTime.ParseExact(ExpirationDate, BaseController.ShortDateFormat, CultureInfo.InvariantCulture),
                Number = Number,
                IsActive = IsActive.GetValueOrDefault(true),
                Owner = userRepository.GetUserByUsername(Owner),
                Balance = Balance.Where(money => money.Value != null).Select(model => model.GetEntity(currencyRepository)).ToList()
            };
        }

        public void SetEntity(Account account)
        {
            if (account == null)
            {
                return;
            }

            Number = account.Number;
            Owner = account.Owner.Username;
            ExpirationDate = account.ExpirationDate.ToString(BaseController.ShortDateFormat, CultureInfo.InvariantCulture);
            IsActive = account.IsActive;
            Balance = new List<MoneyModel>();

            foreach (var money in account.Balance)
            {
                Balance.Add(new MoneyModel(money));
            }
        }
    }

    public enum State { Ok, Low, Blocked }
}