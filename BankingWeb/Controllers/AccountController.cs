using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        //
        // GET: /Account/Balance

        [Authorize]
        public ActionResult Balance()
        {
            return View(CurrentUser.Accounts.Select(account => new AccountModel(account)).ToList());
        }

        [HttpGet]
        public ActionResult Add(string username)
        {
            var balance = _currencyRepository.GetPossibleMoneys().Select(money => new MoneyModel(money)).ToList();

            return View(new AccountModel { Owner = username, Number = _accountRepository.GenerateNumber(), Balance = balance});
        }

        [HttpPost]
        public ActionResult Add(AccountModel accountModel)
        {
            _accountRepository.Add(accountModel.GetEntity(_userRepository, _currencyRepository));
            return RedirectToAction("All", "User");
        }
    }
}
