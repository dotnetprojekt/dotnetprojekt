using FakturyMVC.App_Start;
using FakturyMVC.Models;
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

            // Don't do this in production!
            if (model.Email == "admin@admin.com" && model.Password == "password")
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
            }

            // user authN failed
            ModelState.AddModelError("", "Błędny email lub hasło.");
            return View();
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
        public async Task<ActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

          //  var user = new AppUser
          //  {
          //      UserName = model.Email,
         //       Country = model.Country
         //   };

            //TODO
            //var result = await userManager.CreateAsync(user, model.Password);

            //if (result.Succeeded)
           // {
           //     await SignIn(user);
           //     return RedirectToAction("index", "home");
          //  }

           // foreach (var error in result.Errors)
           // {
           //     ModelState.AddModelError("", error);
           // }

            return View();
        }



        public ActionResult LogOut()
        {
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