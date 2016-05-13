using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class Partner
    {
        private int _id = 0;
        private string _firstName = null;
        private string _lastName = null;
        private string _companyName = null;
        private long _vatin = -1;
        private string _address = null;

        public Partner(string firstName, string lastName, string companyName, long vatin, string address, int id = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            Vatin = vatin;
            Address = address;
            Id = id;
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

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public long Vatin
        {
            get { return _vatin; }
            set { _vatin = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
    }
}
