using System;
using System.Collections.Generic;
using System.Globalization;
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
        public const string AdminOrEmployee = Administrator + ", " + Employee;
        public const string Client = "Client";
        public const string DateFormat = "dd.MM.yyyy";
        public const string ShortDateFormat = "MM.yyyy";
        public const int MoneyBarrier = 100;
        public const int MinAgeBarrier = 18;
        public const int MaxAgeBarrier = 120;
        public const int PageCapacity = 10;

        protected DatabaseContext Context;

        public BaseController()
        {
            Context = new DatabaseContext();

            if (_checker != null) return;

            _checker = new DailyChecker(Context);
            new Thread(_checker.Check).Start();
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

        protected void SaveState()
        {
            TempData["ModelState"] = ModelState;
        }

        protected void LoadState()
        {
            var state = TempData["ModelState"] as ModelStateDictionary;

            if (state == null) return;

            foreach (var pair in state)
            {
                ModelState.Add(pair);
            }
            TempData["ModelState"] = null;
        }

        protected void ValidateBirthday(DateTime birthday)
        {
            if (DateTime.Now < birthday.AddYears(MinAgeBarrier))
            {
                ModelState.AddModelError("birthday", String.Format("User must be at least {0} years old", MinAgeBarrier));
            }
            if (DateTime.Now > birthday.AddYears(MaxAgeBarrier))
            {
                ModelState.AddModelError("birthday", String.Format("User must be younger than {0} years old", MaxAgeBarrier));
            }
        }

        protected DateTime ParseDate(string date)
        {
            return DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture);
        }

        protected DateTime ParseShortDate(string date)
        {
            return DateTime.ParseExact(date, ShortDateFormat, CultureInfo.InvariantCulture);
        }
    }
}