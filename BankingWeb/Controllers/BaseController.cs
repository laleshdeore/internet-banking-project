﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BankingDAL;
using BankingDAL.Entities;

namespace BankingWeb.Controllers
{
    public class BaseController: Controller
    {
        protected readonly DatabaseContext Context = new DatabaseContext();

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