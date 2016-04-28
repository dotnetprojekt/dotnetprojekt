using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FakturyMVC.Models;

namespace FakturyMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // invoice search
            return View();
        }

        //[HttpPost]
        public ActionResult SearchInvoice(string invNumber, string vendor, string buyer, string start, string end)
        {
            // search for invoices in database
            InvoicesViewModel model = new InvoicesViewModel();
            List<Invoice> invoices = new List<Invoice>();
            Invoice invoice = new Invoice();

            int invNumberInt = Int32.Parse(invNumber);

            invoice.Number = invNumberInt;
            invoice.Buyer = "Adam";
            invoice.Vendor = "Krzysiek";
            invoice.Date = "krolik";
            invoices.Add(invoice);
            model.Invoices = invoices;
            
            return PartialView("SearchInvoicesResults", model);
        }

        public ActionResult AddInvoice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddInvoice(string date, string title, /*string vendor, string buyer,*/ List<Goods> goods, 
            double? netto, double? brutto, double? discount, double? value, string vfirstname, string vlastname, string vcompany,
            string bfirstname, string blastname, string bcompany)
        {
            return RedirectToAction("Index");
        }

        public ActionResult AddPartner()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AddPartner(string firstName, string lastName, string companyName, int VATIN, string address)
        {
            // send to database (addPartner)
            return RedirectToAction("Index");
        }

        public ActionResult SearchPartner()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult SearchPartnerResults(string firstName, string lastName, string companyName, string vatin, string address)
        {
            // search for invoices in database
            PartnersVievModel model = new PartnersVievModel();
            List<Partner> partners = new List<Partner>();
            Partner partner = new Partner();

            int vatinInt = Int32.Parse(vatin);
            partner.FirstName = "Kamil";
            partner.LastName = "Nowak";
            partner.CompanyName = "Biedronka";
            partner.Vatin = 123;
            partner.Address = "asdada";

            partners.Add(partner);
            model.Partners = partners;

            return PartialView("SearchPartnersResults", model);

        }

        public ActionResult AddRow(int count)
        {
            Row model = new Row();
            model.Number = count;
            return PartialView("TableRow", model);
        }

        public ActionResult InvoiceDetails(int invoiceNumber)
        {
            // get invoice details from database
            InvoiceDetailsViewModel model = new InvoiceDetailsViewModel();

            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem { Text = "Nowa", Value = "new" });
            status.Add(new SelectListItem { Text = "Opłacona", Value = "paid" });
            status.Add(new SelectListItem { Text = "Zarchiwizowana", Value = "archived" });

            List<Goods> goods = new List<Goods>();

            Goods goods1 = new Goods();
            goods1.Name = "Banany";
            goods1.Price = 14.2;
            goods1.Value = 13.1;
            goods1.Tax = 0.15;
            goods1.Amount = 3;

            Goods goods2 = new Goods();
            goods2.Name = "Pierogi";
            goods2.Price = 12.2;
            goods2.Value = 9.1;
            goods2.Tax = 0.2;
            goods2.Amount = 5;

            goods.Add(goods1);
            goods.Add(goods2);

            Partner vendor = new Partner();
            vendor.FirstName = "Adam";
            vendor.LastName = "Nowak";
            vendor.CompanyName = "Biedronka s.a.";
            vendor.Address = "ul. Mleczna 23, Kraków";
            vendor.Vatin = 325324534;

            Partner buyer = new Partner();
            buyer.FirstName = "Krystyna";
            buyer.LastName = "Krzak";
            buyer.CompanyName = "Kaufland s.a.";
            buyer.Address = "ul. Dziwna 22, Warszawa";
            buyer.Vatin = 43554855;

            model.Number = invoiceNumber;
            model.Date = DateTime.Now.ToString("dd/MM/yyyy");
            model.Title = "Faktura za zakupy";
            model.Vendor = vendor;
            model.Buyer = buyer;
            model.Goods = goods;
            model.Netto = 15.5;
            model.Brutto = 19.0;
            model.Discount = 0;
            model.Value = 19.0;
            model.Status = status;
                        
            return View(model);
        }

        public ActionResult ChangeInvoiceStatus(int invoiceNumber, string status)
        {
            // get invoice details from database
            return RedirectToAction("Index");
        }

        public ActionResult AddInvoiceSearchVendors(string firstName, string lastName, string companyName)
        {
            PartnersVievModel model = new PartnersVievModel();
            List<Partner> partnerList = new List<Partner>();

            Partner partner1 = new Partner();
            partner1.FirstName = "Adam";
            partner1.LastName = "Żelazko";
            partner1.CompanyName = "Żabka";
            partner1.Vatin = 12423423;
            partner1.Address = "ul. Mokra 2 Kraków";

            Partner partner2 = new Partner();
            partner2.FirstName = "Michał";
            partner2.LastName = "Żelazko";
            partner2.CompanyName = "Żabka";
            partner2.Vatin = 12423423;
            partner2.Address = "ul. Mokra 2 Kraków";

            partnerList.Add(partner1);
            partnerList.Add(partner2);

            model.Partners = partnerList;

            return PartialView("AddInvoiceSearchVendors", model);
        }

        public ActionResult AddInvoiceSearchBuyers(string firstName, string lastName, string companyName)
        {
            PartnersVievModel model = new PartnersVievModel();
            List<Partner> partnerList = new List<Partner>();

            Partner partner1 = new Partner();
            partner1.FirstName = "Adam";
            partner1.LastName = "Żelazko";
            partner1.CompanyName = "Żabka";
            partner1.Vatin = 12423423;
            partner1.Address = "ul. Mokra 2 Kraków";

            Partner partner2 = new Partner();
            partner2.FirstName = "Michał";
            partner2.LastName = "Żelazko";
            partner2.CompanyName = "Żabka";
            partner2.Vatin = 12423423;
            partner2.Address = "ul. Mokra 2 Kraków";

            partnerList.Add(partner1);
            partnerList.Add(partner2);

            model.Partners = partnerList;

            return PartialView("AddInvoiceSearchBuyers", model);
        }

    }
}