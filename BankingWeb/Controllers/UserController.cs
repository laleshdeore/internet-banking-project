using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI.WebControls;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Models.User;
using BankingWeb.Providers;

namespace BankingWeb.Controllers
{
    public class UserController : BaseController
    {
        private readonly BankMemberProvider _provider = (BankMemberProvider)Membership.Provider;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

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
            return View(_userRepository.GetUsersByRole(_roleRepository.GetRoleByName(Client)));
        }

        [HttpGet]
        public ActionResult Add(string role)
        {
            return View(new UserModel { Role = role });
        }

        [HttpPost]
        public ActionResult Add(UserModel userModel)
        {
            var user = userModel.GetUserEntity(_roleRepository);

            _userRepository.Add(user);
            return RedirectToAction("Add", "Account", new { username = user.Username });
        }

        public ActionResult Delete(long id)
        {
            _userRepository.Delete(_userRepository.GetUserById(id));

            if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.ToString());

            return RedirectToAction("Clients", "User");
        }

        public ActionResult Index(long id)
        {
            return View(new UserModel(_userRepository.GetUserById(id)));
        }
    }
}
