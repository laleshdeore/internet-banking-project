using System;
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

        [Authorize]
        public ActionResult All(long? id)
        {
            var model = new CurrencyRatesModel { Currencies = _currencyRepository.GetCurrencies() };

            LoadState();
            model.Currency = id == null ? model.Currencies[0] : _currencyRepository.GetCurrencyById(id.Value);
            model.Rates = _currencyRepository.GetCurrencyRates(model.Currency);
            return View(model);
        }

        [Authorize(Roles = Administrator)]
        public ActionResult Add()
        {
            return View(new CurrencyModel());
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
        public ActionResult Add(CurrencyModel currencyModel)
        {
            try
            {
                var id = _currencyRepository.Add(currencyModel.GetEntity());

                return RedirectToAction("Edit", "Currency", new { id });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("currency", e.Message);

                return View(currencyModel);
            }
        }

        [Authorize(Roles = Administrator)]
        public ActionResult Edit(long id)
        {
            return View(_currencyRepository.GetCurrencyRates(_currencyRepository.GetCurrencyById(id)));
        }

        [HttpPost]
        [Authorize(Roles = Administrator)]
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

            return RedirectToAction("All", "Currency");
        }

        [Authorize(Roles = Administrator)]
        public ActionResult Delete(long id)
        {
            try
            {
                _currencyRepository.Delete(_currencyRepository.GetCurrencyById(id));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("currency", e.Message);
            }
            SaveState();
            return RedirectToAction("All", "Currency");
        }
    }
}