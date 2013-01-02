using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class UserRepository : DatabaseRepository, IUserRepository
    {
        public UserRepository(DatabaseContext database)
            : base(database)
        {
        }

        public IList<User> GetUsersByRoles(List<Role> roles, Page page)
        {
            var ids = roles.Select(role => role.Id);
            var users = Database.Users.Where(user => ids.Contains(user.Role.Id)).OrderBy(user => user.Id);

            page.Count = users.Count() / page.Capacity + (users.Count() % page.Capacity != 0 ? 1 : 0);
            return users.Skip(page.Capacity * (page.Number - 1)).Take(page.Capacity).ToList();
        }

        public User GetUserById(long id)
        {
            return Database.Users.SingleOrDefault(user => user.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return Database.Users.SingleOrDefault(user => user.Username == username);
        }

        public void AddOrUpdate(User user)
        {
            if (user.Id == 0)
            {
                Database.Users.Add(user);
                Database.SaveChanges();
            }
            else
            {
                Update(GetUserById(user.Id), user);
            }
        }

        public void Delete(User user)
        {
            Database.Users.Remove(user);
            Database.SaveChanges();
        }
    }
}
