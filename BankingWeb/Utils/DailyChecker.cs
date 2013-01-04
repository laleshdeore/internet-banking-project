using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Utils
{
    public class DailyChecker
    {
        public IPaymentRepository PaymentRepository { get; set; }
        public ICurrencyRepository CurrencyRepository { get; set; }

        public void Check()
        {
            while (true)
            {
                if (DateTime.Now.Day == 1)
                {
                    foreach (var autoPayment in PaymentRepository.GetPayments(true))
                    {
                        PaymentRepository.Pay(new Payment
                        {
                            From = autoPayment.From,
                            To = autoPayment.To,
                            Value = autoPayment.Value,
                            Date = DateTime.Now,
                            ServiceIdentifier = autoPayment.ServiceIdentifier
                        }, CurrencyRepository);
                    }
                }
                Thread.Sleep(TimeSpan.FromDays(1));
            }
        }
    }
}