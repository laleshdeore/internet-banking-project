using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models
{
    public class UsersModel
    {
        public IList<User> Users { get; set; }
        public Page Page { get; set; }

        public UsersModel()
        {
            Users = new List<User>();
            Page = new Page();
        }
    }
}