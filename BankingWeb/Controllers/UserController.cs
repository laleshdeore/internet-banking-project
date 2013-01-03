using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Models;
using BankingWeb.Providers;

namespace BankingWeb.Controllers
{
    public class UserController : BaseController
    {
        private readonly BankMemberProvider _provider = (BankMemberProvider)Membership.Provider;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRegionRepository _regionRepository;

        public UserController()
        {
            _userRepository = new UserRepository(Context);
            _roleRepository = new RoleRepository(Context);
            _regionRepository = new RegionRepository(Context);
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

        [HttpPost]
        public ActionResult PersonalCode(PersonalCodeModel personalCodeModel)
        {
            if (personalCodeModel.Confirm == personalCodeModel.New && personalCodeModel.Old == CurrentUser.PersonalCode)
            {
                CurrentUser.PersonalCode = personalCodeModel.New;
                _userRepository.AddOrUpdate(CurrentUser);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PersonalCode()
        {
            return View(new PersonalCodeModel());
        }


        public ActionResult All(int page = 1)
        {
            var roles = new List<Role>();
            var currentPage = new Page { Capacity = PageCapacity, Number = page };

            if (User.IsInRole(Administrator))
            {
                roles.Add(_roleRepository.GetRoleByName(Administrator));
            }
            if (User.IsInRole(Employee) || User.IsInRole(Administrator))
            {
                roles.Add(_roleRepository.GetRoleByName(Employee));
                roles.Add(_roleRepository.GetRoleByName(Client));
            }

            return View(new UsersModel
            {
                Users = _userRepository.GetUsersByRoles(roles, currentPage),
                Page = currentPage
            });
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

            _userRepository.AddOrUpdate(user);
            return RedirectToAction("Add", "Account", new { username = user.Username });
        }

        [HttpPost]
        public ActionResult Region(RegionModel regionModel)
        {
            CurrentUser.Region = _regionRepository.GetRegionByName(regionModel.Name);
            _userRepository.AddOrUpdate(CurrentUser);
            return RedirectToAction("Region", "User");
        }

        public ActionResult Region()
        {
            return View(new RegionModel
            {
                All = _regionRepository.GetRegions(),
                Name = CurrentUser.Region.Name
            });
        }

        public ActionResult Delete(long id)
        {
            _userRepository.Delete(_userRepository.GetUserById(id));

            if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.ToString());

            return RedirectToAction("All", "User");
        }

        public ActionResult Index(long id)
        {
            return View(new UserModel(_userRepository.GetUserById(id)));
        }
    }
}
