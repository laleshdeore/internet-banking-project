using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Controllers;

namespace BankingWeb.Models.User
{
    public class UserModel
    {
        public UserModel(BankingDAL.Entities.User user)
        {
            SetUserEntity(user);
        }

        public UserModel()
        {
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PersonalCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Birthday { get; set; }

        public string Region { get; set; }

        public string Role { get; set; }

        public BankingDAL.Entities.User GetUserEntity(IRoleRepository roleRepository)
        {
            return new BankingDAL.Entities.User
            {
                Username = Username,
                Password = Password,
                PersonalCode = PersonalCode,
                Birthday = DateTime.ParseExact(Birthday, BaseController.DateFormat, CultureInfo.InvariantCulture),
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Role = roleRepository.GetRoleByName(Role)
            };
        }

        public void SetUserEntity(BankingDAL.Entities.User user)
        {
            Username = user.Username;
            Password = user.Password;
            PersonalCode = user.PersonalCode;
            Birthday = user.Birthday.ToString(BaseController.DateFormat, CultureInfo.InvariantCulture);
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Phone = user.Phone;
            Role = user.Role.Name;
        }
    }
}