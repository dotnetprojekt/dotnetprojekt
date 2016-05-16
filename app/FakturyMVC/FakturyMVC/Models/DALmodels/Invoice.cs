using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace FakturyMVC.Models.DALmodels
{
    public class Invoice
    {
        private int _id;
        private string _number;
        private DateTime _dateOfIssue;
        private string _title;
        private Goods _goodsList;
        private float _overallNet;
        private float _overallGross;
        private float _discount;
        private float _overallCost;
        private InvoiceState _status;

        public Invoice(string number, DateTime dateOfIssue, string title, List<Product> goodsList, float discount, InvoiceState status = InvoiceState.New, int id = 0)
        {
            Id = id;
            Number = number;
            DateOfIssue = dateOfIssue;
            Title = title;
            GoodsList = new Goods(goodsList);
            Discount = discount;
            Status = status;

            callculateCosts();
        }

        public Invoice()
        {

        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public DateTime DateOfIssue
        {
            get { return _dateOfIssue; }
            set { _dateOfIssue = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public Goods GoodsList
        {
            get { return _goodsList; }
            set { _goodsList = value; }
        }

        public float OverallNet
        {
            get { return _overallNet; }
            set { _overallNet = (float)Math.Round(Convert.ToDouble(value), 2); }
        }

        public float OverallGross
        {
            get { return _overallGross; }
            set { _overallGross = (float)Math.Round(Convert.ToDouble(value), 2); }
        }

        public float Discount
        {
            get { return _discount; }
            set { _discount = (float)Math.Round(Convert.ToDouble(value), 2); }
        }

        public float OverallCost
        {
            get { return _overallCost; }
            set { _overallCost = (float)Math.Round(Convert.ToDouble(value), 2); }
        }

        public InvoiceState Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public void callculateCosts()
        {
            float totalNet = 0;
            float totalGross = 0;

            foreach (Product good in _goodsList.ProductList)
            {
                totalNet += good.NetValue;
                totalGross += good.GrossValue;
            }

            OverallGross = totalGross;
            OverallNet = totalNet;
            OverallCost = ((float)1 - _discount) * _overallGross;
        }
    }
}