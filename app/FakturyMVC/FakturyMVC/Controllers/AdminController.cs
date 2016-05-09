using FakturyMVC.Models;
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

        public ActionResult BlockUser(int UserId)
        {
            BlockUnblockViewModel model = new BlockUnblockViewModel();
            model.FirstName = "Jan";
            model.LastName = "Kowalski";
            model.Id = 12;
            model.Login = "Janek";
            model.Email = "jkowalkski@kk.pl"; 
            return View(model);
        }

        public ActionResult UnblockUser(int UserId)
        {
            BlockUnblockViewModel model = new BlockUnblockViewModel();
            model.FirstName = "Marek";
            model.LastName = "Kowalski";
            model.Id = 112312;
            model.Login = "Mareczek";
            model.Email = "jkowalkski@kk.pl";
            return View(model);
        }

        public ActionResult UsersManagement()
        {
            UsersManagementViewModel model = new UsersManagementViewModel();
            List<AppUser> users = new List<AppUser>();
            AppUser user = new AppUser();
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
            users.Add(user);

            model.Users = users;
            return View(model);
        }
    }
}