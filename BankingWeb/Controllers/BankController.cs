using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankingDAL.Repository;
using BankingWeb.Models;

namespace BankingWeb.Controllers
{
    public class BankController : BaseController
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IPaymentRepository _paymentRepository;

        public BankController()
        {
            _currencyRepository = new CurrencyRepository(Context);
            _paymentRepository = new PaymentRepository(Context);
        }

        [Authorize]
        public ActionResult Tariff(BankModel bankModel)
        {
            return View(new BankModel(Context.Bank));
        }

        [Authorize(Roles = Administrator)]
        public ActionResult Index()
        {
            return View(new BankModel(Context.Bank));
        }

        [Authorize(Roles = Administrator)]
        [HttpPost]
        public ActionResult Index(BankModel bankModel)
        {
            var bankFromModel = bankModel.GetEntity(_currencyRepository);
            var bank = Context.Bank;

            foreach (var moneyFromModel in bankFromModel.Balance)
            {
                var money = bank.Balance.SingleOrDefault(m => m.Currency.Name == moneyFromModel.Currency.Name);

                if (money != null)
                {
                    money.Value = moneyFromModel.Value;
                }
            }
            bank.Tariff.MonthlyPay.Value = bankFromModel.Tariff.MonthlyPay.Value;
            _paymentRepository.AddOrUpdate(bank);

            return RedirectToAction("Index", "Bank");
        }

    }
}
