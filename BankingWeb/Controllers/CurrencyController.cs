using System.Collections.Generic;
using System.Web.Mvc;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Models;

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

        public ActionResult Add()
        {
            return View(new CurrencyModel());
        }

        [HttpPost]
        public ActionResult Add(CurrencyModel currencyModel)
        {
            return RedirectToAction("Edit", "Currency", new { id = _currencyRepository.Add(currencyModel.GetEntity()) });
        }

        public ActionResult Edit(long id)
        {
            return View(_currencyRepository.GetCurrencyRates(_currencyRepository.GetCurrencyById(id)));
        }

        [HttpPost]
        public ActionResult Edit(IList<CurrencyRate> rates)
        {
            if (rates != null)
            {
                foreach (var model in rates)
                {
                    var rate = _currencyRepository.GetCurrencyRateById(model.Id);

                    rate.First.Value = model.First.Value;
                    rate.Second.Value = model.Second.Value;
                    _currencyRepository.AddOrUpdate(rate);
                }
            }

            return RedirectToAction("Index", "Currency");
        }
    }
}