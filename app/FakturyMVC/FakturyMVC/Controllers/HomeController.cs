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

        // adding invoice - TODO - uncomment adding
        [HttpPost]
        public ActionResult AddInvoice(string date, string title, string invNumber, List<StringProductList> goods, 
            double? netto, double? brutto, double? discount, double? value, string vfirstname, string vlastname, string vcompany,
            string bfirstname, string blastname, string bcompany, long vvatin, long bvatin)
        {
            DateTime tmpDate = new DateTime();
            //DateTime? parsedDate = new DateTime();

            if (!(DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out tmpDate)))
                tmpDate = DateTime.Today;

            List<Product> productList = new List<Product>();

            for (int i = 0; i < goods.Count; i++ )
            {
                if (goods[i].Name != null)
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

            float notNullDiscount;
            
            if (discount == null)
                notNullDiscount = 0;
            else
                notNullDiscount = (float)discount;

            Invoice invoice = new Invoice(invNumber, tmpDate, title, productList, notNullDiscount);
            InvoiceDAL.InvoiceAdd(invoice, vvatin, bvatin);

            return RedirectToAction("Index");
        }

        // view for adding partner - DONE
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

        // view for searching partners - DONE
        public ActionResult SearchPartner()
        {
            return View();
        }

        // searching partners - DONE
        public ActionResult SearchPartnerResults(string firstName, string lastName, string companyName, string vatin)
        {
            if (firstName == "")
                firstName = null;
            if (lastName == "")
                lastName = null;
            if (companyName == "")
                companyName = null;

            long vatinLong = -1;
            if (vatin != "")
                long.TryParse(vatin, out vatinLong);

            List<Partner> partnerList = new List<Partner>();
            partnerList = PartnerDAL.PartnerSearch(firstName, lastName, companyName, vatinLong);
            PartnersVievModel model = new PartnersVievModel();
            model.Partners = partnerList;

            return PartialView("SearchPartnersResults", model);

        }

        // adding product row - DONE
        public ActionResult AddRow(int count)
        {
            Row model = new Row();
            model.Number = count;
            return PartialView("TableRow", model);
        }

        // return invoice deatils - TODO
        public ActionResult InvoiceDetails(string invoiceId)
        {
            int id = -1;
            Int32.TryParse(invoiceId, out id);







            // get invoice details from database
            InvoiceDetailsViewModel model = new InvoiceDetailsViewModel();

            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem { Text = "Nowa", Value = "new", Selected=true });
            status.Add(new SelectListItem { Text = "Opłacona", Value = "paid", Selected = false });
            status.Add(new SelectListItem { Text = "Zarchiwizowana", Value = "archived", Selected = false });

            List<GoodsList> goods = new List<GoodsList>();

            GoodsList goods1 = new GoodsList();
            goods1.Name = "Banany";
            goods1.Amount = id;
            goods1.Value = 13.1;
            goods1.Tax = 0.15;
            goods1.Gross = 50.5;
            

            GoodsList goods2 = new GoodsList();
            goods2.Name = "Pierogi";
            goods2.Amount = id;
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

            model.Number = "aaa";
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

        // searching vendors for adding invoice - DONE
        public ActionResult AddInvoiceSearchVendors(string firstName, string lastName, string companyName, string vatin)
        {
            if (firstName == "")
                firstName = null;
            if (lastName == "")
                lastName = null;
            if (companyName == "")
                companyName = null;

            long vatinLong = -1;
            if (vatin != "")
                long.TryParse(vatin, out vatinLong);

            List<Partner> partnerList = new List<Partner>();
            partnerList = PartnerDAL.PartnerSearch(firstName, lastName, companyName, vatinLong);
            PartnersVievModel model = new PartnersVievModel();
            model.Partners = partnerList;

            return PartialView("AddInvoiceSearchVendors", model);
        }

        // searching buyers for adding invoice - DONE
        public ActionResult AddInvoiceSearchBuyers(string firstName, string lastName, string companyName, string vatin)
        {
            if (firstName == "")
                firstName = null;
            if (lastName == "")
                lastName = null;
            if (companyName == "")
                companyName = null;

            long vatinLong = -1;
            if (vatin != "")
                long.TryParse(vatin, out vatinLong);

            List<Partner> partnerList = new List<Partner>();
            partnerList = PartnerDAL.PartnerSearch(firstName, lastName, companyName, vatinLong);
            PartnersVievModel model = new PartnersVievModel();
            model.Partners = partnerList;

            return PartialView("AddInvoiceSearchBuyers", model);
        }

    }

}