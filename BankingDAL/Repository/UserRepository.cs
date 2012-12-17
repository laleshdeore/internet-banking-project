using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _db;

        public UserRepository(DatabaseContext db)
        {
            _db = db;
        }

        public IList<User> GetUsersByRole(Role role)
        {
            return new List<User>(_db.Users.Where(user => user.Role.Id == role.Id));
        }

        public User GetUserById(long id)
        {
            return _db.Users.SingleOrDefault(user => user.Id == id);
        }

        public void Add(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void Delete(User user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
        }
    }
}
