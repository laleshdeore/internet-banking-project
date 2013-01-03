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

        public PaymentController()
        {
            _currencyRepository = new CurrencyRepository(Context);
            _paymentRepository = new PaymentRepository(Context);
            _accountRepository = new AccountRepository(Context);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Pay(PaymentModel paymentModel)
        {
            _paymentRepository.Pay(paymentModel.GetEntity(_currencyRepository, _accountRepository), _currencyRepository);
            return RedirectToAction("History", "Payment");
        }

        [Authorize]
        public ActionResult Pay()
        {
            var model = new PaymentModel { Currencies = _currencyRepository.GetCurrencies() };

            foreach (var account in CurrentUser.Accounts)
            {
                model.Accounts.Add(new AccountModel(account));
            }
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