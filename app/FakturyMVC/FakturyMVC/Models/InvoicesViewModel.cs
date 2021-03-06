﻿using FakturyMVC.Models.DALmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models
{
    public class InvoiceApp
    {
        public string Number { get; set; }
        public string Vendor { get; set; }
        public string Buyer { get; set; }
        public string Date { get; set; }
    }

    public class InvoicesViewModel
    {
        //public List<InvoiceApp> Invoices { get; set; }
        public List<InvoiceGenerals> InvoiceGeneralsList { get; set; }
        public bool FirstPage { get; set; }
        public bool LastPage { get; set; }
    }
}