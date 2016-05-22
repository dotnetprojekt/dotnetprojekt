using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models
{
    public class GoodsList
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        // cena jednostkowa
        public double Price { get; set; }
        // cena netto całości
        public double Value { get; set; }
        public double Tax { get; set; }
        // cena brutto całości
        public double Gross { get; set; }
    }
}

