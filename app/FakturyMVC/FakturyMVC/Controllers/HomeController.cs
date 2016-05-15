using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FakturyMVC.Models;
using FakturyMVC.Models.DALmodels;
using System.Globalization;

namespace FakturyMVC.Controllers
{
    public class HomeController : Controller
    {
        // get view for invoice search - DONE
        public ActionResult Index()
        {
            // invoice search
            return View();
        }

        //search invoices - DONE - not checked
        public ActionResult SearchInvoice(string invNumber, string title, string start, string end, string vname, string vlastname, string vcompany, string vvatin,
            string bname, string blastname, string bcompany, string bvatin, string minValue, string maxValue)
        {
            if (invNumber == "")
                invNumber = null;

            if (title == "")
                title = null;

            DateTime tmpStart = new DateTime();
            DateTime tmpEnd = new DateTime();
            DateTime? startDate = new DateTime();
            DateTime? endDate = new DateTime();

            if (!( DateTime.TryParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out tmpStart )))
                startDate = null;
            else
                startDate = tmpStart;
            
            if (!( DateTime.TryParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out tmpEnd )))
                endDate = null;
            else
                endDate = tmpEnd;            


            long vendorVatin = -1;
            long buyerVatin = -1;

            if (!(vvatin == ""))
                vendorVatin = long.Parse(vvatin);

            if (!(bvatin == ""))
                buyerVatin = long.Parse(bvatin);

            if (vname == "")
                vname = null;
            if (vlastname == "")
                vlastname = null;
            if (vcompany == "")
                vcompany = null;

            if (bname == "")
                bname = null;
            if (blastname == "")
                blastname = null;
            if (bcompany == "")
                bcompany = null;

            Partner vendor = new Partner(vname, vlastname, vcompany, vendorVatin, null);
            Partner buyer = new Partner(bname, blastname, bcompany, buyerVatin, null);

            float minValueFloat = -1;
            float maxValueFloat = -1;

            if (!(minValue == ""))
                minValueFloat = float.Parse(minValue);

            if (!(maxValue == ""))
                maxValueFloat = float.Parse(maxValue);
                       

            List<InvoiceGenerals> invoiceGeneralsList = new List<InvoiceGenerals>();
            invoiceGeneralsList = InvoiceDAL.InvoiceSearch(invNumber, startDate, endDate, title, minValueFloat, maxValueFloat, vendor, buyer);
            InvoicesViewModel model = new InvoicesViewModel();
            model.InvoiceGeneralsList = invoiceGeneralsList;

            return PartialView("SearchInvoicesResults", model);
        }

        // return view for adding invoice - DONE
        public ActionResult AddInvoice()
        {
            string invNumber = InvoiceDAL.GetInvoiceNumber();
            AddInvoiceViewModel model = new AddInvoiceViewModel();
            model.InvNumber = invNumber;
            return View("AddInvoice", model);
        }

        // adding invoice - TODO - parse date and floats
        [HttpPost]
        public ActionResult AddInvoice(string date, string title, string invNumber, List<StringProductList> goods, 
            double? netto, double? brutto, double? discount, double? value, string vfirstname, string vlastname, string vcompany,
            string bfirstname, string blastname, string bcompany)
        {
            //DateTime? todayDate = new DateTime();
            //todayDate = DateTime.Parse(date);
            List<Product> productList = new List<Product>();

            for (int i = 0; i < goods.Count; i++ )
            {
                if (goods[i].Name != "")
                {
                    Product product = new Product();
                    product.Name = goods[i].Name;
                    product.Amount = Int32.Parse(goods[i].Amount, CultureInfo.InvariantCulture);
                    product.UnitPrice = float.Parse(goods[i].Price, CultureInfo.InvariantCulture);
                    product.NetValue = float.Parse(goods[i].Value, CultureInfo.InvariantCulture);
                    product.Tax = float.Parse(goods[i].Tax, CultureInfo.InvariantCulture);
                    product.GrossValue = float.Parse(goods[i].Gross, CultureInfo.InvariantCulture);
                    productList.Add(product);
                }
            }




                //Invoice invoice = new Invoice(invNumber, todayDate, title

                //InvoiceDAL.InvoiceAdd

                // convert string do date?? TODO
                return RedirectToAction("Index");
        }

        public ActionResult AddPartner()
        {
            return View();
        }
        
        // addPartner to database - DONE
        [HttpPost]
        public ActionResult AddPartner(string firstName, string lastName, string companyName, long VATIN, string address)
        {
            Partner partner = new Partner(firstName, lastName, companyName, VATIN, address);
            PartnerDAL.PartnerAdd(partner);

            return RedirectToAction("Index");
        }

        public ActionResult SearchPartner()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult SearchPartnerResults(string firstName, string lastName, string companyName, string vatin)
        {
            // search for invoices in database
            PartnersVievModel model = new PartnersVievModel();
            List<PartnerApp> partners = new List<PartnerApp>();
            PartnerApp partner = new PartnerApp();

            long vatinInt = -1;
            if (vatin != "")
                vatinInt = long.Parse(vatin);


            
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

        public ActionResult InvoiceDetails(string invoiceNumber)
        {
            // get invoice details from database
            InvoiceDetailsViewModel model = new InvoiceDetailsViewModel();

            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem { Text = "Nowa", Value = "new", Selected=true });
            status.Add(new SelectListItem { Text = "Opłacona", Value = "paid", Selected = false });
            status.Add(new SelectListItem { Text = "Zarchiwizowana", Value = "archived", Selected = false });

            List<GoodsList> goods = new List<GoodsList>();

            GoodsList goods1 = new GoodsList();
            goods1.Name = "Banany";
            goods1.Amount = 3;
            goods1.Value = 13.1;
            goods1.Tax = 0.15;
            goods1.Gross = 50.5;
            

            GoodsList goods2 = new GoodsList();
            goods2.Name = "Pierogi";
            goods2.Amount = 3;
            goods2.Value = 11.1;
            goods2.Tax = 0.12;
            goods2.Gross = 34.5;

            goods.Add(goods1);
            goods.Add(goods2);

            PartnerApp vendor = new PartnerApp();
            vendor.FirstName = "Adam";
            vendor.LastName = "Nowak";
            vendor.CompanyName = "Biedronka s.a.";
            vendor.Address = "ul. Mleczna 23, Kraków";
            vendor.Vatin = 325324534;

            PartnerApp buyer = new PartnerApp();
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
            model.GoodsList = goods;
            model.Netto = 15.5;
            model.Brutto = 19.0;
            model.Discount = 0;
            model.Value = 19.0;
            model.Status = status;
                        
            return View(model);
        }

        public ActionResult ChangeInvoiceStatus(long invoiceNumber, string status)
        {
            // get invoice details from database
            return RedirectToAction("Index");
        }

        public ActionResult AddInvoiceSearchVendors(string firstName, string lastName, string companyName, string vatin)
        {
            PartnersVievModel model = new PartnersVievModel();
            List<PartnerApp> partnerList = new List<PartnerApp>();

            PartnerApp partner1 = new PartnerApp();
            partner1.FirstName = "Adam";
            partner1.LastName = "Żelazko";
            partner1.CompanyName = "Żabka";
            partner1.Vatin = 12423423;
            partner1.Address = "ul. Mokra 2 Kraków";

            PartnerApp partner2 = new PartnerApp();
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

        public ActionResult AddInvoiceSearchBuyers(string firstName, string lastName, string companyName, string vatin)
        {
            PartnersVievModel model = new PartnersVievModel();
            List<PartnerApp> partnerList = new List<PartnerApp>();

            PartnerApp partner1 = new PartnerApp();
            partner1.FirstName = "Adam";
            partner1.LastName = "Żelazko";
            partner1.CompanyName = "Żabka";
            partner1.Vatin = 12423423;
            partner1.Address = "ul. Mokra 2 Kraków";

            PartnerApp partner2 = new PartnerApp();
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