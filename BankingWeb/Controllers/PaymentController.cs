using System;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
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

        [Authorize]
        public ActionResult AutoPayment()
        {
            return View(_paymentRepository.GetAutoPaymentsByUser(CurrentUser));
        }

        [HttpPost]
        [Authorize]
        public ActionResult Pay(int? serviceId, PaymentModel paymentModel)
        {
            var payment = paymentModel.GetEntity(_currencyRepository, _accountRepository);

            if (payment.To == null)
            {
                ModelState.AddModelError("recipient", "Can't find recipient account");
            }
            if (payment.From != null &&
                !payment.From.ExpirationDate.ToString(ShortDateFormat).Equals(paymentModel.From.ExpirationDate))
            {
                ModelState.AddModelError("expirationDate", "Wrong expiration date");
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
                return RedirectToAction("Pay", "Payment", new { serviceId });
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
        public ActionResult History(string from, string to, long? id, int page = 1)
        {
            var currentPage = new Page { Capacity = PageCapacity, Number = page };
            var fromDate = DateTime.Now;
            var toDate = DateTime.Now.AddMonths(-1);
            var user = CurrentUser;
            var all = User.IsInRole(Administrator) || User.IsInRole(Employee);

            if (all && id != null)
            {
                all = false;
                user = _userRepository.GetUserById(id.Value);
            }

            if (from != null)
            {
                fromDate = ParseDate(from);
            }
            if (to != null)
            {
                toDate = ParseDate(to).AddDays(1).AddMilliseconds(-1);
            }

            return View(new PaymentsModel
            {
                Payments = all ? _paymentRepository.GetPayments(fromDate, toDate, currentPage) : _paymentRepository.GetPaymentsByUser(user, fromDate, toDate, currentPage),
                From = fromDate.ToString(DateFormat),
                To = toDate.ToString(DateFormat),
                Page = currentPage
            });
        }

        [Authorize]
        public ActionResult Delete(long id)
        {
            var payment = _paymentRepository.GetPaymentById(id);

            if (payment != null && payment.IsAutomatic)
            {
                _paymentRepository.Delete(payment);
            }
            return RedirectToAction("AutoPayment", "Payment");
        }
    }
}