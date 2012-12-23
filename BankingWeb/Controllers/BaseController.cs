using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankingDAL;
using BankingDAL.Entities;

namespace BankingWeb.Controllers
{
    public class BaseController : Controller
    {
        public const string Administrator = "Administrator";
        public const string Employee = "Employee";
        public const string Client = "Client";
        public const string DateFormat = "dd.MM.yyyy";
        public const string ShortDateFormat = "MM.yyyy";

        protected DatabaseContext Context
        {
            get { return new DatabaseContext(); }
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