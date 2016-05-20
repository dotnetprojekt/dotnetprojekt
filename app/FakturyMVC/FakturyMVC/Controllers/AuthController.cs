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

namespace FakturyMVC.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {


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
            
           
            if (UserDAL.UserLogin(model.Login, model.Password))
            {
                List<User> tmp = UserDAL.UserSearch(null, null, model.Login, null, null, null, null);
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
                ModelState.AddModelError("", "Błędny email lub hasło.");
                return View();
            }
            // Don't do this in production!
            /*if (model.Email == "admin@admin.com" && model.Password == "password")
            {
                var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, "Waldek"),
                new Claim(ClaimTypes.Email, "a@a.com"),
                new Claim(ClaimTypes.Country, "Polska"),
                new Claim(ClaimTypes.Role, "Admin")
            },
                    "ApplicationCookie");



                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }*/

            // user authN failed

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

            List<User> tmp = UserDAL.UserSearch(null, null, model.Login, null, null, true);
            if (tmp.Any())
            {
                return View();
            }
            tmp = UserDAL.UserSearch(null, null, null, model.Email, null, true);
            if (tmp.Any())
            {
                return View();
            }

            User tmpUser = new User(model.FirstName, model.LastName, model.Login, model.Password, model.Email, UserStatus.User);
            UserDAL.UserAdd(tmpUser);

            //TODO
            //var result = await userManager.CreateAsync(user, model.Password);

            //if (result.Succeeded)
            // {
            //     await SignIn(user);
            //     
            //  }

            // foreach (var error in result.Errors)
            // {
            //     ModelState.AddModelError("", error);
            // }
            return RedirectToAction("index", "home");
          //  return View();
        }



        public ActionResult LogOut()
        {

            //UserDAL.UserLogout();
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