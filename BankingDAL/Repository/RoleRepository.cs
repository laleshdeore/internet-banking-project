using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext _db;

        public RoleRepository(DatabaseContext db)
        {
            _db = db;
        }

        public Role GetRoleByName(string name)
        {
            return _db.Roles.FirstOrDefault(role => role.Name == name);
        }
    }
}
