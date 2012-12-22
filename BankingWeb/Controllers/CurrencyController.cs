using System.Web.Mvc;
using BankingDAL.Repository;

namespace BankingWeb.Controllers
{
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController()
        {
            _currencyRepository = new CurrencyRepository(Context);
        }

        public ActionResult Index()
        {
            return View(_currencyRepository.GetCurrencyRates());
        }
    }
}