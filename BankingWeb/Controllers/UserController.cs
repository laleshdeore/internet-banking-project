﻿using System;
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
        public ActionResult Login(LoginModel loginModel)
        {
            if (_provider.ValidateUser(loginModel.Username, loginModel.Password))
            {
                FormsAuthentication.SetAuthCookie(loginModel.Username, loginModel.RememberMe);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("username", "There is no user with such username / password pair");

            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public ActionResult PersonalCode(PersonalCodeModel personalCodeModel)
        {
            if (personalCodeModel.Confirm == personalCodeModel.New && personalCodeModel.Old == CurrentUser.PersonalCode)
            {
                CurrentUser.PersonalCode = personalCodeModel.New;
                _userRepository.AddOrUpdate(CurrentUser);

                return RedirectToAction("Index", "Home");
            }
            if (personalCodeModel.Confirm != personalCodeModel.New)
            {
                ModelState.AddModelError("confirm", "Wrong confirmation code");
            }
            if (personalCodeModel.Old != CurrentUser.PersonalCode)
            {
                ModelState.AddModelError("old", "Wrong old code");
            }
            return View(personalCodeModel);
        }

        [Authorize]
        public ActionResult PersonalCode()
        {
            return View(new PersonalCodeModel());
        }

        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult All(int page = 1)
        {
            var roles = new List<Role>();
            var currentPage = new Page { Capacity = PageCapacity, Number = page };

            if (User.IsInRole(Administrator))
            {
                roles.Add(_roleRepository.GetRoleByName(Administrator));
                roles.Add(_roleRepository.GetRoleByName(Employee));
            }
            if (User.IsInRole(Employee) || User.IsInRole(Administrator))
            {
                roles.Add(_roleRepository.GetRoleByName(Client));
            }

            LoadState();
            return View(new UsersModel
            {
                Users = _userRepository.GetUsersByRoles(roles, currentPage),
                Page = currentPage
            });
        }

        [HttpGet]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Add(string role)
        {
            return View(new UserModel { Role = role, Regions = _regionRepository.GetRegions() });
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(UserModel userModel)
        {
            var user = userModel.GetUserEntity(_roleRepository, _regionRepository);

            ValidateBirthday(user.Birthday);
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepository.AddOrUpdate(user);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("user", e.Message);
            }

            if (!ModelState.IsValid)
            {
                userModel.Regions = _regionRepository.GetRegions();
                return View(userModel);
            }

            return user.Role.Name == Client ? RedirectToAction("Add", "Account", new {username = user.Username}) : RedirectToAction("All", "User");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Region(RegionModel regionModel)
        {
            CurrentUser.Region = _regionRepository.GetRegionByName(regionModel.Name);
            _userRepository.AddOrUpdate(CurrentUser);
            return RedirectToAction("Region", "User");
        }

        [Authorize]
        public ActionResult Region()
        {
            return View(new RegionModel
            {
                All = _regionRepository.GetRegions(),
                Name = CurrentUser.Region.Name
            });
        }

        [Authorize]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Delete(long id)
        {
            var user = _userRepository.GetUserById(id);
            var isCurrent = user == CurrentUser;

            if (user.Role.Name.Equals(Administrator) && user.Role.Users.Count == 1)
            {
                ModelState.AddModelError("role", "There is only one administrator");
            }

            if (ModelState.IsValid)
            {
                _userRepository.Delete(user);
                if (isCurrent)
                {
                    return RedirectToAction("LogOff", "User");
                }
            }
            SaveState();
            return RedirectToAction("All", "User");
        }

        [Authorize]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Index(long id)
        {
            return View(_userRepository.GetUserById(id));
        }

        [HttpPost]
        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Edit(long id, UserModel userModel)
        {
            var userFromModel = userModel.GetUserEntity(_roleRepository, _regionRepository);
            var user = _userRepository.GetUserById(id);


            ValidateBirthday(userFromModel.Birthday);
            try
            {
                if (ModelState.IsValid)
                {
                    user.Birthday = userFromModel.Birthday;
                    user.Email = userFromModel.Email;
                    user.FirstName = userFromModel.FirstName;
                    user.LastName = userFromModel.LastName;
                    user.Phone = userFromModel.Phone;
                    user.Password = userFromModel.Password;
                    _userRepository.AddOrUpdate(user);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("user", e.Message);
            }

            if (!ModelState.IsValid)
            {
                userModel.Regions = _regionRepository.GetRegions();
                return View(userModel);
            }

            return RedirectToAction("All", "User");
        }

        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Edit(long id)
        {
            return View(new UserModel(_userRepository.GetUserById(id)) { Regions = _regionRepository.GetRegions() });
        }
    }
}
