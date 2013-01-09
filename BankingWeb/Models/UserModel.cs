using System;
using System.Collections.Generic;
using System.Globalization;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Controllers;

namespace BankingWeb.Models
{
    public class UserModel
    {
        public UserModel(User user)
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

        public IList<AccountModel> Accounts { get; set; }

        public IList<Region> Regions { get; set; } 

        public User GetUserEntity(IRoleRepository roleRepository, IRegionRepository regionRepository)
        {
            return new User
            {
                Username = Username,
                Password = Password,
                PersonalCode = PersonalCode,
                Birthday = DateTime.ParseExact(Birthday, BaseController.DateFormat, CultureInfo.InvariantCulture),
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                Role = roleRepository.GetRoleByName(Role),
                Region = regionRepository.GetRegionByName(Region)
            };
        }

        public void SetUserEntity(User user)
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
            Accounts = new List<AccountModel>();

            foreach (var account in user.Accounts)
            {
                Accounts.Add(new AccountModel(account));
            }
        }
    }
}