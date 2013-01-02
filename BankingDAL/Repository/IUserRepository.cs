using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IUserRepository
    {
        IList<User> GetUsersByRoles(List<Role> roles, Page page);
        User GetUserById(long id);
        User GetUserByUsername(string username);
        void AddOrUpdate(User user);
        void Delete(User user);
    }
}
