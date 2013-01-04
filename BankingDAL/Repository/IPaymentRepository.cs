using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IPaymentRepository
    {
        IList<Payment> GetPaymentsByUser(User user, Page page); 

        void Pay(Payment payment, ICurrencyRepository currencyRepository);

        void AddOrUpdate(Service service);

        Service GetServiceById(long id);

        IList<Service> GetServices();
    }
}
