using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BankingDAL;

namespace BankingWeb.Providers
{
    public class BankRoleProvider : RoleProvider
    {
        private readonly DatabaseContext _context = new DatabaseContext();

        public override bool IsUserInRole(string username, string roleName)
        {
            return _context.Users.First(user => user.Username == username).Role.Name == roleName;
        }
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        public override string[] GetRolesForUser(string username)
        {
            return new[] { _context.Users.First(user => user.Username == username).Role.Name };
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
    }
}