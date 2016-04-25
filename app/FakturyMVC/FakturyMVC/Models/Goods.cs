using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models
{
    public class Goods
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public double Value { get; set; }
        public double Tax { get; set; }
    }
}

