using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IUserRepository
    {
        IList<User> GetUsersByRole(Role role);
        User GetUserById(long id);
        void Add(User user);
        void Delete(User user);
    }
}
