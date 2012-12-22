using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class UserRepository : DatabaseRepository, IUserRepository
    {
        public UserRepository(DatabaseContext database) : base(database)
        {
        }

        public IList<User> GetUsersByRole(Role role)
        {
            return new List<User>(Database.Users.Where(user => user.Role.Id == role.Id));
        }

        public User GetUserById(long id)
        {
            return Database.Users.SingleOrDefault(user => user.Id == id);
        }

        public void Add(User user)
        {
            Database.Users.Add(user);
            Database.SaveChanges();
        }

        public void Delete(User user)
        {
            Database.Users.Remove(user);
            Database.SaveChanges();
        }
    }
}
