using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IPaymentRepository
    {
        void Pay(Payment payment, ICurrencyRepository currencyRepository);
    }
}
