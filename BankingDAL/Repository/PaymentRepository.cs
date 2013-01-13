using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class PaymentRepository : DatabaseRepository, IPaymentRepository
    {
        public PaymentRepository(DatabaseContext database)
            : base(database)
        {
        }

        public IList<Payment> GetPaymentsByUser(User user, DateTime from, DateTime to, Page page)
        {
            var accountIds = user.Accounts.Select(account => account.Id).ToList();

            return Database.Payments.Where(p => (accountIds.Contains(p.From.Id) || accountIds.Contains(p.To.Id)) && !p.IsAutomatic).Where(p => (p.Date <= from) && (to <= p.Date)).OrderByDescending(p => p.Date).ToList();
        }

        public IList<Payment> GetPayments(DateTime from, DateTime to, Page page)
        {
            return Database.Payments.Where(p => (p.Date <= from) && (to <= p.Date)).OrderByDescending(p => p.Date).ToList();
        }

        public IList<Payment> GetAutoPaymentsByUser(User user)
        {
            var accountIds = user.Accounts.Select(account => account.Id).ToList();

            return Database.Payments.Where(p => (accountIds.Contains(p.From.Id) || accountIds.Contains(p.To.Id)) && p.IsAutomatic).ToList();
        }

        public IList<Payment> GetAutoPayments()
        {
            return Database.Payments.Where(payment => payment.IsAutomatic).ToList();
        }

        public void Pay(Payment payment, ICurrencyRepository currencyRepository)
        {
            payment.State = PaymentState.Pending;
            AddOrUpdate(payment);

            if (payment.IsAutomatic)
            {
                return;
            }
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

                if (payMoney.Currency != payment.Value.Currency)
                {
                    var bankFrom = Database.Bank.Balance.SingleOrDefault(m => m.Currency == payMoney.Currency);
                    var bankTo = Database.Bank.Balance.SingleOrDefault(m => m.Currency == payment.Value.Currency);

                    if (bankFrom != null && bankTo != null)
                    {
                        bankFrom.Value -= payMoney.Value;
                        bankTo.Value += payment.Value.Value;
                    }
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

        public void AddOrUpdate(Bank bank)
        {
            if (bank.Id == 0)
            {
                Database.Banks.Add(bank);
                Database.SaveChanges();
            }
            else
            {
                Update(Database.Banks.SingleOrDefault(b => b.Id == bank.Id), bank);
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

        public void Delete(Payment payment)
        {
            Database.Payments.Remove(payment);
            SaveAllChanges();
        }
    }
}
