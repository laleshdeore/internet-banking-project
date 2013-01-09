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

        IList<Payment> GetPayments(bool isAutomatic); 

        void Pay(Payment payment, ICurrencyRepository currencyRepository);

        void AddOrUpdate(Service service);

        void AddOrUpdate(Payment payment);

        Payment GetPaymentById(long id);

        Service GetServiceById(long id);

        IList<Service> GetServices();

        void Delete(Service service);
    }
}
