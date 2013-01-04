using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class PaymentRepository : DatabaseRepository, IPaymentRepository
    {
        public PaymentRepository(DatabaseContext database) : base(database)
        {
        }

        public IList<Payment> GetPaymentsByUser(User user, Page page)
        {
            var accountIds = user.Accounts.Select(account => account.Id).ToList();

            return Database.Payments.Where(p => accountIds.Contains(p.From.Id) || accountIds.Contains(p.To.Id)).ToList();
        }

        public void Pay(Payment payment, ICurrencyRepository currencyRepository)
        {
            payment.State = PaymentState.Pending;
            Database.Payments.Add(payment);
            SaveAllChanges();

            if (!payment.From.IsActive || !payment.To.IsActive)
            {
                throw new Exception("One of accounts is blocked");
            }

            var toMoney = payment.To.Balance.SingleOrDefault(money => money.Currency.Id == payment.Value.Currency.Id);
            var fromMoney = payment.From.Balance.SingleOrDefault(money => money.Currency.Id == payment.Value.Currency.Id);

            if (toMoney == null)
            {
                toMoney = payment.To.Balance.First();
            }

            if (fromMoney == null)
            {
                fromMoney = payment.From.Balance.First();
            }

            var payMoney = currencyRepository.Convert(payment.Value, toMoney.Currency);

            fromMoney.Value -= payMoney.Value;
            toMoney.Value += payMoney.Value;
            payment.State = PaymentState.Completed;
            SaveAllChanges();
        }

        public void AddOrUpdate(Service service)
        {
            if (service.Id == 0)
            {
                Database.Services.Add(service);
                Database.SaveChanges();
            }
            else
            {
                Update(GetServiceById(service.Id), service);
            }
        }

        public Service GetServiceById(long id)
        {
            return Database.Services.SingleOrDefault(service => service.Id == id);
        }

        public IList<Service> GetServices()
        {
            return Database.Services.ToList();
        }
    }
}
