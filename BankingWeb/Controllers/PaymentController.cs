using System.Web.Mvc;
using BankingDAL.Repository;
using BankingWeb.Models;

namespace BankingWeb.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly ICurrencyRepository _currencyRepository;

        public PaymentController()
        {
            _currencyRepository = new CurrencyRepository(Context);
        }

        [HttpPost]
        public ActionResult Pay(PaymentModel paymentModel)
        {
            return RedirectToAction("History", "Account");
        }

        public ActionResult Pay()
        {
            var model = new PaymentModel();

            foreach (var account in CurrentUser.Accounts)
            {
                model.Accounts.Add(new AccountModel(account));
            }
            foreach (var money in _currencyRepository.GetPossibleMoneys())
            {
                model.Amount.Add(new MoneyModel(money));
            }
            return View(model);
        }
    }
}