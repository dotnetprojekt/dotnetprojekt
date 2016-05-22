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

            List<User> tmp = UserDAL.Instance.UserSearch(null, null, model.Login, null, null, null, null);
            //if (UserDAL.UserLogin(model.Login, model.Password))
            if(tmp.Any())
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

            List<User> tmp = UserDAL.Instance.UserSearch(null, null, model.Login, null, null, null);
            if (tmp.Any())
            {
                ModelState.AddModelError("", "Błędny login.");
                return View();
            }
            tmp = UserDAL.Instance.UserSearch(null, null, null, model.Email, null, null);
            if (tmp.Any())
            {
                ModelState.AddModelError("", "Błędny email.");
                return View();
            }

            User tmpUser = new User(model.FirstName, model.LastName, model.Login, model.Password, model.Email, UserStatus.User);
            UserDAL.Instance.UserAdd(tmpUser);

            
            return RedirectToAction("index", "home");
        }



        public ActionResult LogOut()
        {
            var identity = (ClaimsIdentity)User.Identity;
            foreach (var claim in identity.Claims)
            {
                if (claim.Type == "NameIdentifier")
                {
                    UserDAL.Instance.UserLogout(claim.Value);
                    break; 
                }
            }
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