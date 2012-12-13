using System;
using System.Web.Mvc;
using System.Web.Security;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Providers;

namespace BankingWeb.Controllers
{
    public class UserController : BaseController
    {
        private readonly BankMemberProvider _provider = (BankMemberProvider) Membership.Provider;
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public UserController()
        {
            _userRepository = new UserRepository(Context);
            _roleRepository = new RoleRepository(Context);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            if (_provider.ValidateUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Index", "Home");
            }


            return RedirectToAction("Login", "User");
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Clients()
        {
            return View(model: _userRepository.GetUsersByRole(_roleRepository.GetRoleByName(Client)));
        }

        [HttpPost]
        public void Add()
        {
            _userRepository.Add(new User { FirstName = "qwe", LastName = "rty", Role = _roleRepository.GetRoleByName(Client), Birthday = new DateTime()});
        }
    }
}
