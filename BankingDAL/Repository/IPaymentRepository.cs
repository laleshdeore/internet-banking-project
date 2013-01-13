using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IPaymentRepository
    {
        IList<Payment> GetPaymentsByUser(User user, DateTime from, DateTime to, Page page);

        IList<Payment> GetPayments(DateTime from, DateTime to, Page page);

        IList<Payment> GetAutoPaymentsByUser(User user);

        IList<Payment> GetAutoPayments(); 

        void Pay(Payment payment, ICurrencyRepository currencyRepository);

        void AddOrUpdate(Service service);

        void AddOrUpdate(Payment payment);

        void AddOrUpdate(Bank bank);

        Payment GetPaymentById(long id);

        Service GetServiceById(long id);

        IList<Service> GetServices();

        void Delete(Service service);

        void Delete(Payment payment);
    }
}
