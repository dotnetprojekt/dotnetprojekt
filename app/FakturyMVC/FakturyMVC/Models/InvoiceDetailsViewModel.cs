using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FakturyMVC.Models
{
    public class InvoiceDetailsViewModel
    {
        public int Number { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public Partner Vendor { get; set; }
        public Partner Buyer { get; set; }
        public List<Goods> Goods { get; set; }
        public double Netto { get; set; }
        public double Brutto { get; set; }
        public double Discount { get; set; }
        public double Value { get; set; }
        public List<SelectListItem> Status { get; set; }
    }
}