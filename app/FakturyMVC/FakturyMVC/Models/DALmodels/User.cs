using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakturyMVC.Controllers;

namespace FakturyMVC.Models.DALmodels
{
    public class User
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private string _login;
        private string _passwordHash;
        private string _email;
        private bool _isAdmin;
        private bool _isLogged;
        private UserStatus _status;

        public User(string firstName, string lastName, string login, string password, string email, UserStatus status = UserStatus.Guest, bool isAdmin = false, bool isLogged = false, int id = 0)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            PasswordHash = password;
            Email = email;
            IsAdmin = isAdmin;
            IsLogged = isLogged;
            Status = status;
        }

        private User()
        {

        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string PasswordHash
        {
            get { return _passwordHash; }
            set { _passwordHash = Utils.GetStringSha256Hash(value); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }

        public bool IsLogged
        {
            get { return _isLogged; }
            set { _isLogged = value; }
        }

        public UserStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}