using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using BankingDAL.Repository;
using BankingWeb.Models;

namespace BankingWeb.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUserRepository _userRepository;

        public AccountController()
        {
            _accountRepository = new AccountRepository(Context);
            _currencyRepository = new CurrencyRepository(Context);
            _userRepository = new UserRepository(Context);
        }

        [Authorize]
        public ActionResult Balance()
        {
            return View(CurrentUser.Accounts.Select(account => new AccountModel(account)).ToList());
        }

        [HttpGet]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Add(string username)
        {
            var balance = _currencyRepository.GetPossibleMoneys().Select(money => new MoneyModel(money)).ToList();

            return View(new AccountModel { Owner = username, Number = _accountRepository.GenerateNumber(), Balance = balance });
        }

        [HttpPost]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Add(AccountModel accountModel)
        {
            try
            {
                _accountRepository.AddOrUpdate(accountModel.GetEntity(_userRepository, _currencyRepository));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("account", e.Message);
                return View(accountModel);
            }

            return RedirectToAction("All", "User");
        }

        [HttpGet]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Edit(long id)
        {
            return View(new AccountModel(_accountRepository.GetAccountById(id)));
        }

        [HttpPost]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Edit(long id, AccountModel accountModel)
        {
            var accountFromModel = accountModel.GetEntity(_userRepository, _currencyRepository);
            var account = _accountRepository.GetAccountById(id);

            if (account != null)
            {
                account.ExpirationDate = accountFromModel.ExpirationDate;
                foreach (var money in accountFromModel.Balance)
                {
                    var accountMoney = account.Balance.SingleOrDefault(m => m.Currency.Name == money.Currency.Name);

                    if (accountMoney == null)
                    {
                        account.Balance.Add(money);
                    }
                    else
                    {
                        accountMoney.Value = money.Value;
                    }
                }
                _accountRepository.AddOrUpdate(account);
                return RedirectToAction("Index", "User", new { id = account.Owner.Id });
            }

            return RedirectToAction("All", "User");
        }

        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Block(long id)
        {
            var account = _accountRepository.GetAccountById(id);

            if (account != null)
            {
                account.IsActive = !account.IsActive;
                _accountRepository.AddOrUpdate(account);
                return RedirectToAction("Index", "User", new { id = account.Owner.Id });
            }
            return RedirectToAction("All", "User");
        }

        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Delete(long id)
        {
            var account = _accountRepository.GetAccountById(id);

            if (account != null)
            {
                _accountRepository.Delete(account);
                return RedirectToAction("Index", "User", new { id = account.Owner.Id });
            }
            return RedirectToAction("All", "User");
        }
    }
}
