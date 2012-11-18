using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankingWeb.Controllers
{
    public class AccountController : Controller
    {
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
