using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturyMVC.Models.DALmodels
{
    public class InvoiceDetails
    {
        public Invoice Invoice;
        public Partner Vendor;
        public Partner Buyer;

        public InvoiceDetails(Invoice invoice, Partner vendor, Partner buyer)
        {
            Invoice = invoice;
            Vendor = vendor;
            Buyer = buyer;
        }
    }
}