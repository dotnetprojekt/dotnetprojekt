using FakturyMVC.Models;
using FakturyMVC.Models.DALmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FakturyMVC.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult AddUser()
        {
            return View();
        }

        public ActionResult BlockUser(string UserLogin)
        {
            BlockUnblockViewModel model = new BlockUnblockViewModel();
            UserDAL.Instance.UserBlock(UserLogin);
            List<User> tmp = UserDAL.Instance.UserSearch(null, null, UserLogin, null, null, null, null);

            model.FirstName = tmp.First().FirstName;
            model.LastName = tmp.First().LastName;
            model.Id = tmp.First().Id;
            model.Login = tmp.First().Login;
            model.Email = tmp.First().Email;
            return RedirectToAction("UsersManagement", model);
        }

        public ActionResult UnblockUser(string UserLogin)
        {
            BlockUnblockViewModel model = new BlockUnblockViewModel();
            UserDAL.Instance.UserUnblock(UserLogin);
            List<User> tmp = UserDAL.Instance.UserSearch(null, null, UserLogin, null, null, null, null);

            model.FirstName = tmp.First().FirstName;
            model.LastName = tmp.First().LastName;
            model.Id = tmp.First().Id;
            model.Login = tmp.First().Login;
            model.Email = tmp.First().Email;
            return RedirectToAction("UsersManagement", model);
        }

        public ActionResult UsersManagement()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();
            List<User> usersList = UserDAL.Instance.UserSearch(null, null, null, null, null, null);

            List<AppUser> users = new List<AppUser>();
            AppUser user;
            for (int i = 0; i < usersList.Count; i++)
            {
                user = new AppUser();
                user.Id = usersList[i].Id;
                user.FirstName = usersList[i].FirstName;
                user.LastName = usersList[i].LastName;
                user.Email = usersList[i].Email;
                user.IsAdmin = usersList[i].IsAdmin;
                user.Login = usersList[i].Login;
                user.Status = usersList[i].Status == UserStatus.Blocked ? 0 : 1;
                users.Add(user);
            }

            model.Users = users;
            return View(model);
        }
    }
}