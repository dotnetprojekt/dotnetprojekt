using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakturyMVC.Models.DALmodels
{
    public class InvoiceGenerals
    {
        private int _id;
        private string _number;
        private DateTime _dateOfIssue;
        private string _vendorData;
        private string _buyerData;
        private string _title;
        private float _overallCost;
        private InvoiceState _status;

        public InvoiceGenerals(string number, DateTime dateOfIssue, string vendor, string buyer, string title, float overallCost, InvoiceState status, int id = 0)
        {
            Id = id;
            Number = number;
            DateOfIssue = dateOfIssue;
            VendorData = vendor;
            BuyerData = buyer;
            Title = title;
            OverallCost = overallCost;
            Status = status;
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

        public string VendorData
        {
            get { return _vendorData; }
            set { _vendorData = value; }
        }

        public string BuyerData
        {
            get { return _buyerData; }
            set { _buyerData = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
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
    }
}