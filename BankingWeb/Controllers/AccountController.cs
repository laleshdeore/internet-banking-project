using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankingDAL.Repository;

namespace BankingWeb.Controllers
{
    public class AccountController : BaseController
    {
        private IAccountRepository _accountRepository;

        public AccountController()
        {
            _accountRepository = new AccountRepository(Context);
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
    }
}
