using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models
{
    public class Invoice
    {
        public string Number { get; set; }
        public string Vendor { get; set; }
        public string Buyer { get; set; }
        public string Date { get; set; }
    }

    public class InvoicesViewModel
    {
        public List<Invoice> Invoices { get; set; }
    }
}