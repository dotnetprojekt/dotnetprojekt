using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class Product
    {
        private string _name;
        private int _amount;
        private float _unitPrice;
        private float _tax;
        private float _netValue;
        private float _grossValue;

        public Product()
        {

        }

        public Product(string name, int amount, float unitPrice, float tax)
        {
            Name = name;
            Amount = amount;
            UnitPrice = unitPrice;
            Tax = tax;
            GrossValue = _amount * _unitPrice;
            NetValue = ((float)1 - _tax) * _grossValue;
        }

        [XmlElement("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [XmlElement("amount")]
        public int  Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        [XmlElement("unitPrice")]
        public float UnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = (float)Math.Round(Convert.ToDouble(value),2); }
        }

        [XmlElement("tax")]
        public float Tax
        {
            get { return _tax; }
            set { _tax = (float)Math.Round(Convert.ToDouble(value), 2); }
        }

        [XmlElement("netValue")]
        public float NetValue
        {
            get { return _netValue; }
            set { _netValue = (float)Math.Round(Convert.ToDouble(value), 2); }
        }

        [XmlElement("grossValue")]
        public float GrossValue
        {
            get { return _grossValue; }
            set { _grossValue = (float)Math.Round(Convert.ToDouble(value), 2); }
        }
    }
}
