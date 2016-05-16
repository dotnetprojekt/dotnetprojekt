using FakturyMVC.Models.DALmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models
{
    public class PartnerApp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public long Vatin { get; set; }
        public string Address { get; set; }
    }

    public class PartnersVievModel
    {
        public List<Partner> Partners { get; set; }
    }
}