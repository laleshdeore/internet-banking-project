using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models
{
    public class MoneyModel
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public double? Value { get; set; }

        public MoneyModel() { }

        public MoneyModel(Money entity)
        {
            SetEntity(entity);
        }

        public Money GetEntity(ICurrencyRepository currencyRepository)
        {
            return new Money
            {
                Currency = currencyRepository.GetCurrencyByName(Name),
                Value = Value.GetValueOrDefault(0)
            };
        }

        public void SetEntity(Money money)
        {
            Name = money.Currency.Name;
            Symbol = money.Currency.Symbol;
            Value = money.Value;
        }
    }
}