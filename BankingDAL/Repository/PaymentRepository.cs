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

            return Database.Payments.Where(p => accountIds.Contains(p.From.Id) || accountIds.Contains(p.To.Id) && !p.IsAutomatic).ToList();
        }

        public IList<Payment> GetPayments(bool isAutomatic)
        {
            return Database.Payments.Where(payment => payment.IsAutomatic == isAutomatic).ToList();
        }

        public void Pay(Payment payment, ICurrencyRepository currencyRepository)
        {
            payment.State = PaymentState.Pending;
            AddOrUpdate(payment);

            try
            {
                if (!payment.From.IsActive || !payment.To.IsActive)
                {
                    payment.State = PaymentState.Canceled;
                    AddOrUpdate(payment);
                    throw new Exception("One of accounts is blocked");
                }

                var toMoney = payment.To.Balance.SingleOrDefault(money => money.Currency.Id == payment.Value.Currency.Id);
                var fromMoney =
                    payment.From.Balance.SingleOrDefault(money => money.Currency.Id == payment.Value.Currency.Id);

                if (toMoney == null)
                {
                    toMoney = payment.To.Balance.First();
                }

                if (fromMoney == null)
                {
                    fromMoney = payment.From.Balance.First();
                }

                var payMoney = currencyRepository.Convert(payment.Value, fromMoney.Currency);

                if (payMoney.Value > fromMoney.Value)
                {
                    payment.State = PaymentState.Canceled;
                    AddOrUpdate(payment);
                    throw new Exception("Not enough money for pay");
                }

                fromMoney.Value -= payMoney.Value;
                toMoney.Value += currencyRepository.Convert(payMoney, toMoney.Currency).Value;
                SaveAllChanges();

                payment.State = PaymentState.Completed;
                AddOrUpdate(payment);
            }
            catch (Exception e)
            {
                payment.State = PaymentState.Canceled;
                AddOrUpdate(payment);
                throw e;
            }
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

        public void AddOrUpdate(Payment payment)
        {
            if (payment.Id == 0)
            {
                Database.Payments.Add(payment);
                Database.SaveChanges();
            }
            else
            {
                Update(GetPaymentById(payment.Id), payment);
            }
        }

        public Payment GetPaymentById(long id)
        {
            return Database.Payments.SingleOrDefault(payment => payment.Id == id);
        }

        public Service GetServiceById(long id)
        {
            return Database.Services.SingleOrDefault(service => service.Id == id);
        }

        public IList<Service> GetServices()
        {
            return Database.Services.ToList();
        }

        public void Delete(Service service)
        {
            Database.Services.Remove(service);
            SaveAllChanges();
        }
    }
}
