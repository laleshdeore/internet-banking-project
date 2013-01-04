using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BankingDAL;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Utils;

namespace BankingWeb.Controllers
{
    public class BaseController : Controller
    {
        private static DailyChecker _checker;

        public const string Administrator = "Administrator";
        public const string Employee = "Employee";
        public const string Client = "Client";
        public const string All = "All";
        public const string DateFormat = "dd.MM.yyyy";
        public const string ShortDateFormat = "MM.yyyy";
        public const int MoneyBarrier = 100;
        public const int PageCapacity = 10;

        protected DatabaseContext Context;

        public BaseController()
        {
            Context = new DatabaseContext();

            if (_checker == null)
            {
                _checker = new DailyChecker
                {
                    CurrencyRepository = new CurrencyRepository(Context),
                    PaymentRepository = new PaymentRepository(Context)
                };
                new Thread(_checker.Check).Start();
            }
        }

        public User CurrentUser
        {
            get
            {
                User user = null;

                if (User.Identity.IsAuthenticated)
                {
                    user = Context.Users.First(u => u.Username == User.Identity.Name);
                }
                return user;
            }
        }
    }
}