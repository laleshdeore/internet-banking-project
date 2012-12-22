using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class RoleRepository : DatabaseRepository, IRoleRepository
    {
        public RoleRepository(DatabaseContext database) : base(database)
        {
        }

        public Role GetRoleByName(string name)
        {
            return Database.Roles.FirstOrDefault(role => role.Name == name);
        }
    }
}
