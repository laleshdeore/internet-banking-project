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
            var balance = new List<MoneyModel>();
            var currencies = _currencyRepository.GetCurrencies();

            foreach (var currency in currencies)
            {
                balance.Add(new MoneyModel());
            }
            return View(new AccountModel { Owner = username });
        }

        [HttpPost]
        public ActionResult Add(AccountModel accountModel)
        {
            return RedirectToAction("Clients", "User");
        }
    }
}
