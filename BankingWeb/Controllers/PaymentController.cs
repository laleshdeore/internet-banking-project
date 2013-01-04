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
            _paymentRepository.Pay(paymentModel.GetEntity(_currencyRepository, _accountRepository, _userRepository), _currencyRepository);

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