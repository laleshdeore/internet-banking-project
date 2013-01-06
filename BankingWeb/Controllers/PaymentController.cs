using System;
using System.Web.Mvc;
using BankingDAL.Repository;
using BankingWeb.Models;

namespace BankingWeb.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public PaymentController()
        {
            _currencyRepository = new CurrencyRepository(Context);
            _paymentRepository = new PaymentRepository(Context);
            _accountRepository = new AccountRepository(Context);
            _userRepository = new UserRepository(Context);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Pay(PaymentModel paymentModel)
        {
            var payment = paymentModel.GetEntity(_currencyRepository, _accountRepository, _userRepository);

            if (payment.To == null)
            {
                ModelState.AddModelError("recipient", "Can't find recipient account");
            }
            if (paymentModel.PersonalCode != CurrentUser.PersonalCode)
            {
                ModelState.AddModelError("personalCode", "Personal code is different");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _paymentRepository.Pay(payment, _currencyRepository);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("payment", e.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                SaveState();
                return RedirectToAction("Pay", "Payment");
            }

            return RedirectToAction("History", "Payment");
        }

        [Authorize]
        public ActionResult Pay(int? serviceId)
        {
            var model = new PaymentModel { Currencies = _currencyRepository.GetCurrencies() };

            if (serviceId != null && serviceId != 0)
            {
                model.Service = _paymentRepository.GetServiceById(serviceId.Value);
            }
            foreach (var account in CurrentUser.Accounts)
            {
                model.Accounts.Add(new AccountModel(account));
            }

            LoadState();
            return View(model);
        }

        [Authorize]
        public ActionResult History(int page = 1)
        {
            var currentPage = new Page { Capacity = PageCapacity, Number = page };

            return View(new PaymentsModel
            {
                Payments = _paymentRepository.GetPaymentsByUser(CurrentUser, currentPage),
                Page = currentPage
            });
        }
    }
}