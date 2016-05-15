using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models
{
    public class StringProductList
    {
        public string Name { get; set; }
        public string Amount { get; set; }
        // cena jednostkowa
        public string Price { get; set; }
        // cena netto całości
        public string Value { get; set; }
        public string Tax { get; set; }
        // cena brutto całości
        public string Gross { get; set; }
    }
}