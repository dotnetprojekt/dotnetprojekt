using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FakturyMVC.Models;
using FakturyMVC.Models.DALmodels;
using System.Globalization;
using System.Data.SqlClient;

namespace FakturyMVC.Controllers
{
    public class HomeController : Controller
    {
        // to add paging:
        // 1. uncomment two buttons in SearchInvoicesResults/SearchPartnersResults
        // 2. uncomment in javascript SearchInvoices() and SearchPartners() functions methods show()
        // 3. uncomment functions InvoiceSearch/PartnerSearch methods with pageNumber and rowsPerPage


        // get view for invoice search - DONE
        public ActionResult Index()
        {       
            return View();
        }

        //search invoices - DONE - not checked
        public ActionResult SearchInvoice(string invNumber, string title, string start, string end, string vname, string vlastname, string vcompany, string vvatin,
            string bname, string blastname, string bcompany, string bvatin, string minValue, string maxValue, string page, int rowsPerPage)
        {
            int pgNumber = 0;
            if (page == "none")
                pgNumber = 1;
            else if (page == "prev")
            {
                pgNumber = (int)TempData["pageNumber"] - 1;
                invNumber = (string)TempData["invNumber"];
                title = (string)TempData["title"];
                start = (string)TempData["start"];
                end = (string)TempData["end"];
                vname = (string)TempData["vname"];
                vlastname = (string)TempData["vlastname"];
                vcompany = (string)TempData["vcompany"];
                vvatin = (string)TempData["vvatin"];
                bname = (string)TempData["bname"];
                blastname = (string)TempData["blastname"];
                bcompany = (string)TempData["bcompany"];
                bvatin = (string)TempData["bvatin"];
                minValue = (string)TempData["minValue"];
                maxValue = (string)TempData["maxValue"];
                rowsPerPage = (int)TempData["rowsPerPage"];
            }
            else if (page == "next")
            {
                pgNumber = (int)TempData["pageNumber"] + 1;
                invNumber = (string)TempData["invNumber"];
                title = (string)TempData["title"];
                start = (string)TempData["start"];
                end = (string)TempData["end"];
                vname = (string)TempData["vname"];
                vlastname = (string)TempData["vlastname"];
                vcompany = (string)TempData["vcompany"];
                vvatin = (string)TempData["vvatin"];
                bname = (string)TempData["bname"];
                blastname = (string)TempData["blastname"];
                bcompany = (string)TempData["bcompany"];
                bvatin = (string)TempData["bvatin"];
                minValue = (string)TempData["minValue"];
                maxValue = (string)TempData["maxValue"];
                rowsPerPage = (int)TempData["rowsPerPage"];
            }

            TempData["pageNumber"] = pgNumber;
            TempData["rowsPerPage"] = rowsPerPage;

            TempData["title"] = title;
            TempData["start"] = start;
            TempData["end"] = end;
            TempData["vname"] = vname;
            TempData["vlastname"] = vlastname;
            TempData["vcompany"] = vcompany;
            TempData["vvatin"] = vvatin;
            TempData["bname"] = bname;
            TempData["blastname"] = blastname;
            TempData["bcompany"] = bcompany;
            TempData["bvatin"] = bvatin;
            TempData["minValue"] = minValue;
            TempData["maxValue"] = maxValue;



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


            ResultSet<InvoiceGenerals> invoiceGeneralsList = new ResultSet<InvoiceGenerals>();
            // UNCOMMENT FOR PAGING
            //invoiceGeneralsList = InvoiceDAL.Instance.InvoiceSearch(invNumber, startDate, endDate, title, minValueFloat, maxValueFloat, vendor, buyer, pgNumber, rowsPerPage);
            invoiceGeneralsList = InvoiceDAL.Instance.InvoiceSearch(invNumber, startDate, endDate, title, minValueFloat, maxValueFloat, vendor, buyer);
            InvoicesViewModel model = new InvoicesViewModel();
            model.InvoiceGeneralsList = invoiceGeneralsList.Items;
            model.FirstPage = false;
            model.LastPage = false;

            if (pgNumber == 1)
                model.FirstPage = true;
            if (pgNumber == 2) // hardcoded for now
                model.LastPage = true;

            return PartialView("SearchInvoicesResults", model);
        }

        // return view for adding invoice - DONE
        public ActionResult AddInvoice()
        {
            //string invNumber = InvoiceDAL.GetInvoiceNumber();
            string invNumber = InvoiceDAL.Instance.GetInvoiceNumber();
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

            if (!(DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out tmpDate)))
                tmpDate = DateTime.Today;

            List<Product> productList = new List<Product>();

            for (int i = 0; i < goods.Count; i++ )
            {
                if (goods[i].Name != null)
                {
                    Product product = new Product
                        (
                            goods[i].Name, 
                            Int32.Parse(goods[i].Amount, CultureInfo.InvariantCulture),
                            float.Parse(goods[i].Price, CultureInfo.InvariantCulture),
                            float.Parse(goods[i].Tax, CultureInfo.InvariantCulture)/100
                        );
                    productList.Add(product);
                }
            }

            float notNullDiscount;
            
            if (discount == null)
                notNullDiscount = 0;
            else
                notNullDiscount = (float)discount;

            Invoice invoice = new Invoice(invNumber, tmpDate, title, productList, notNullDiscount/100);

            try
            {
                //InvoiceDAL.InvoiceAdd(invoice, vvatin, bvatin);
                InvoiceDAL.Instance.InvoiceAdd(invoice, vvatin, bvatin);
            }
            catch (SqlException e)
            {
                string exc = e.ToString();
                SqlExceptionViewModel model = new SqlExceptionViewModel();
                model.Exc = exc;
                return View("SqlExceptionMessage", model);
            }            

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

            try
            {
                PartnerDAL.Instance.PartnerAdd(partner);
            }
            catch (SqlException e)
            {
                if(e.Number!=2627)
                {
                    string exc = e.Message;
                    SqlExceptionViewModel model = new SqlExceptionViewModel();
                    model.Exc = exc;
                    return View("SqlExceptionMessage", model);
                }
                else
                {
                    TempData["msg"] = "<script>alert('Partner o podanym VATIN już istnieje!');</script>";
                    return View("AddPartner");
                }
            }
            

            return RedirectToAction("Index");
        }

        // view for searching partners - DONE
        public ActionResult SearchPartner()
        {
            return View();
        }

        // searching partners - DONE
        public ActionResult SearchPartnerResults(string firstName, string lastName, string companyName, string vatin, string page, int rowsPerPage = 0)
        {
            int pgNumber = 0;
            if (page == "none")
                pgNumber = 1;
            else if (page == "prev")
            {
                pgNumber = (int)TempData["pageNumber"] - 1;
                firstName = (string)TempData["firstName"];
                lastName = (string)TempData["lastName"];
                companyName = (string)TempData["companyName"];
                vatin = (string)TempData["vatin"];
                rowsPerPage = (int)TempData["rowsPerPage"];
            }                
            else if (page == "next")
            {
                pgNumber = (int)TempData["pageNumber"] + 1;
                firstName = (string)TempData["firstName"];
                lastName = (string)TempData["lastName"];
                companyName = (string)TempData["companyName"];
                vatin = (string)TempData["vatin"];
                rowsPerPage = (int)TempData["rowsPerPage"];
            }

            TempData["pageNumber"] = pgNumber;
            TempData["rowsPerPage"] = rowsPerPage;

            TempData["firstName"] = firstName;
            TempData["lastName"] = lastName;
            TempData["companyName"] = companyName;
            TempData["vatin"] = vatin;

            if (firstName == "")
                firstName = null;
            if (lastName == "")
                lastName = null;
            if (companyName == "")
                companyName = null;

            long vatinLong = -1;
            if (vatin != "")
                long.TryParse(vatin, out vatinLong);

            ResultSet<Partner> partnerList = new ResultSet<Partner>();
            //partnerList = PartnerDAL.Instance.PartnerSearch(firstName, lastName, companyName, vatinLong);
            // UNCOMMENT FOR PAGING
            partnerList = PartnerDAL.Instance.PartnerSearch(firstName, lastName, companyName, vatinLong, pgNumber, rowsPerPage);
            PartnersVievModel model = new PartnersVievModel();
            model.Partners = partnerList.Items;
            model.FirstPage = false;
            model.LastPage = false;

            if (pgNumber == 1)
                model.FirstPage = true;
            if (partnerList.IsLastPage == true)
                model.LastPage = true;

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

            //InvoiceDetails model = InvoiceDAL.InvoiceGetDetails(id);
            InvoiceDetails model = InvoiceDAL.Instance.InvoiceGetDetails(id);
            return View(model);
        }

        // set invoice status to paid - DONE - not checked
        public ActionResult ChangeInvoiceStatusToPaid(int id)
        {
            //InvoiceDAL.InvoiceSetPaid(id);
            InvoiceDAL.Instance.InvoiceSetPaid(id);
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

            ResultSet<Partner> partnerList = PartnerDAL.Instance.PartnerSearch(firstName, lastName, companyName, vatinLong);
            PartnersVievModel model = new PartnersVievModel();
            model.Partners = partnerList.Items;

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

            ResultSet<Partner> partnerList = PartnerDAL.Instance.PartnerSearch(firstName, lastName, companyName, vatinLong);
            PartnersVievModel model = new PartnersVievModel();
            model.Partners = partnerList.Items;

            return PartialView("AddInvoiceSearchBuyers", model);
        }

    }

}