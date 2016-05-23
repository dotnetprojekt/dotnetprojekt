using FakturyMVC.App_Start;
using FakturyMVC.Models;
using FakturyMVC.Models.DALmodels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web.Security;

namespace FakturyMVC.Controllers

{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        public AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(this.User as ClaimsPrincipal);
            }
        }


        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var model = new LogInModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult LogIn(LogInModel model)
        {
            

            if (!ModelState.IsValid)
            {
                return View();
            }

            List<User> tmp = UserDAL.Instance.UserSearch(null, null, model.Login, null, null, null, null);
            if (UserDAL.Instance.UserLogin(model.Login, model.Password))
            {
               // List<User> tmp = UserDAL.UserSearch(null, null, model.Login, null, null, null, null);
                var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, tmp.First().FirstName),
                new Claim(ClaimTypes.Role, tmp.First().IsAdmin ? "Admin" : "User"),
                 new Claim(ClaimTypes.NameIdentifier, tmp.First().Login)
                },
                    "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));

            } 
            else
            {
                ModelState.AddModelError("", "Błędny login lub hasło.");
                return View();
            }

        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return  ctx.Authentication;
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            User tmpUser = new User(model.FirstName, model.LastName, model.Login, model.Password, model.Email, UserStatus.User);
            try
            {
                UserDAL.Instance.UserAdd(tmpUser);
            }
            catch(SqlException e)
            {
                if (e.Number != 2627)
                {
                    string exc = e.Message;
                    SqlExceptionViewModel modelsql = new SqlExceptionViewModel();
                    modelsql.Exc = exc;
                    return View("SqlExceptionMessage", modelsql);
                }
                else
                {
                    TempData["msg"] = "<script>alert('Podany login istnieje w bazie!');</script>";
                    return View("Register");
                }
            }

            
            return RedirectToAction("index", "home");
        }



        public ActionResult LogOut()
        {
            var identity = (ClaimsIdentity)User.Identity;
            UserDAL.Instance.UserLogout(CurrentUser.Login);               
                            
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("index", "home");
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }
    }
}