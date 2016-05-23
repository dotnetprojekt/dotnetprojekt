using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;

namespace FakturyMVC.Models
{
    public class AppUser
    {
        private ClaimsPrincipal claimsPrincipal;

        public int Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Login { get; set; }

        public bool IsAdmin { get; set; }
    
        public int Status { get; set; }

    }
}