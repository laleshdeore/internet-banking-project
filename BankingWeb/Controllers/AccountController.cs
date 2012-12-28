using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankingDAL.Repository;
using BankingWeb.Models.Account;
using BankingWeb.Models.User;

namespace BankingWeb.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public AccountController()
        {
            _accountRepository = new AccountRepository(Context);
            _currencyRepository = new CurrencyRepository(Context);
        }

        //
        // GET: /Account/Balance

        public ActionResult Balance()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add(string username)
        {
            var currencies = _currencyRepository.GetCurrencies();
            var balance = currencies.Select(currency => new MoneyModel {Symbol = currency.Symbol}).ToList();

            return View(new AccountModel { Owner = username, Number = _accountRepository.GenerateNumber(), Balance = balance});
        }

        [HttpPost]
        public ActionResult Add(AccountModel accountModel)
        {
            return RedirectToAction("Clients", "User");
        }
    }
}
