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
            UserDAL.UserBlock(UserLogin);
            List<User> tmp = UserDAL.UserSearch(null, null, UserLogin, null, null, null, null);

            model.FirstName = tmp.First().FirstName;
            model.LastName = tmp.First().LastName;
            model.Id = tmp.First().Id;
            model.Login = tmp.First().Login;
            model.Email = tmp.First().Email;
            return View(model);
        }

        public ActionResult UnblockUser(string UserLogin)
        {
            BlockUnblockViewModel model = new BlockUnblockViewModel();
            UserDAL.UserUnblock(UserLogin);
            List<User> tmp = UserDAL.UserSearch(null, null, UserLogin, null, null, null, null);

            model.FirstName = tmp.First().FirstName;
            model.LastName = tmp.First().LastName;
            model.Id = tmp.First().Id;
            model.Login = tmp.First().Login;
            model.Email = tmp.First().Email;
            return View(model);
        }

        public ActionResult UsersManagement()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();
            List<User> usersList = UserDAL.UserSearch(null, null, null, null, null, null);

            List<AppUser> users = new List<AppUser>();
            AppUser user;
            for (int i = 0; i < usersList.Count; i++)
            {
                user = new AppUser();
                user.Id = usersList[i].Id;
                user.FirstName = usersList[i].FirstName;
                user.Email = usersList[i].Email;
                user.IsAdmin = usersList[i].IsAdmin;
                user.Login = usersList[i].Login;
                user.Status = usersList[i].Status == UserStatus.Blocked ? 0 : 1; 
            }

           /*user = new AppUser();
            user.Id = 1;
            user.FirstName = "Antek";
            user.Email = "email1@email.com";
            user.IsAdmin = false;
            user.LastName = "Antalski";
            user.Login = "Antkowiak";
            user.Status = 1;
            users.Add(user);

            user = new AppUser();
            user.Id = 2;
            user.FirstName = "Mietek";
            user.Email = "email2@email.com";
            user.IsAdmin = false;
            user.LastName = "Mietkowski";
            user.Login = "Mieciu";
            user.Status = 0;
            users.Add(user);

            user = new AppUser();
            user.Id = 3;
            user.FirstName = "Paweł";
            user.Email = "email3@email.com";
            user.IsAdmin = true;
            user.LastName = "Kowalski";
            user.Login = "Jasiek";
            user.Status = 1;
            users.Add(user);*/

            model.Users = users;
            return View(model);
        }
    }
}