using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models
{
    public class BankModel
    {
        public BankModel()
        {
            Balance = new List<MoneyModel>();
        }

        public BankModel(Bank entity) : this()
        {
            SetEntity(entity);
        }

        public MoneyModel MonthlyPay { get; set; }

        public IList<MoneyModel> Balance { get; set; }

        public void SetEntity(Bank entity)
        {
            foreach (var money in entity.Balance)
            {
                Balance.Add(new MoneyModel(money));
            }

            MonthlyPay = new MoneyModel(entity.Tariff.MonthlyPay);
        }

        public Bank GetEntity(ICurrencyRepository currencyRepository)
        {
            var entity = new Bank
                             {
                                 Balance = Balance.Select(m => m.GetEntity(currencyRepository)).ToList(),
                                 Tariff = new Tariff()
                             };

            entity.Tariff.MonthlyPay = MonthlyPay.GetEntity(currencyRepository);
            return entity;
        }
    }
}