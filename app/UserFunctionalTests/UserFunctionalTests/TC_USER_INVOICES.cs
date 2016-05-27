using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using UserFunctionalTests.UIMapInvoicesClasses;


namespace UserFunctionalTests
{
    /// <summary>
    /// Summary description for TC_USER_INVOICES
    /// </summary>
    [CodedUITest]
    public class TC_USER_INVOICES
    {
        public TC_USER_INVOICES()
        {
        }

        [TestMethod]
        public void TC_USER_INVOICES_001()
        {
            /** Poprawne dodanie nowej faktury przez użytkownika w Systemie Zarządzania Fakturami */
            BrowserWindow browser = testInit();
            browser.Refresh();
            this.UIMapInvoices.invoicesLoginAction();
            this.UIMapInvoices.addPartnerInvoiceAction();
            this.UIMapInvoices.addPartnerInvoiceAction();
            this.UIMapInvoices.addInvoiceAction();
            //string invNum = this.UIMapInvoices.fillInvoiceFieldsAction();
            string invNum = this.UIMapInvoices.fillInvoiceWithOneProduct();
            this.UIMapInvoices.invoiceFieldsWithOneProductAssertion();
            this.UIMapInvoices.invoiceSubmitAction();
            this.UIMapInvoices.logoutAction();
        }

        [TestMethod]
        public void TC_USER_INVOICES_002()
        {
            /** Sprawdzenie poprawności wyliczonych wartości faktury (suma netto, suma brutto, ogółem) w Systemie Zarządzania Fakturami */
            BrowserWindow browser = testInit();
            this.UIMapInvoices.invoicesLoginAction();
            /*this.UIMapInvoices.addPartnerInvoiceAction();
            this.UIMapInvoices.addPartnerInvoiceAction();*/
            this.UIMapInvoices.addInvoiceAction();
            string invNum = this.UIMapInvoices.fillInvoiceFieldsAction();
            this.UIMapInvoices.invoiceVerificationAssetion();
            this.UIMapInvoices.invoiceSubmitAction();
            this.UIMapInvoices.logoutAction();
        }

        [TestMethod]
        public void TC_USER_INVOICES_003()
        {
            /** Wyszukanie partnera biznesowego */
            BrowserWindow browser = testInit();
            this.UIMapInvoices.invoicesLoginAction();
            string v1 = this.UIMapInvoices.addPartnerToSearchAction();
            string v2 = this.UIMapInvoices.addPartnerToSearchAction();
            this.UIMapInvoices.addInvoiceAction();
            this.UIMapInvoices.fillInvoiceForSearchPartnersAction(v1, v2);
            this.UIMapInvoices.findPartnerAssertion(v1, v2);
            this.UIMapInvoices.logoutAction();
        }

        [TestMethod]
        public void TC_USER_INVOICES_005()
        {
            /** Test wylistowania faktur użytkownika w Systemie Zarządzania Fakturami. */
            BrowserWindow browser = testInit();
            this.UIMapInvoices.invoicesLoginAction();
            /*this.UIMapInvoices.addPartnerInvoiceAction();
            this.UIMapInvoices.addPartnerInvoiceAction();*/
            this.UIMapInvoices.addInvoiceAction();
            string invNum = this.UIMapInvoices.fillInvoiceWithOneProduct();
            //this.UIMapInvoices.invoiceAssertions();
            this.UIMapInvoices.invoiceSubmitAction();
            this.UIMapInvoices.searchInvoiceAction("");
            this.UIMapInvoices.listAllInvoicesAssertion();
            this.UIMapInvoices.logoutAction();
        }

        /*[TestMethod]
        public void TC_USER_INVOICES_006()
        {
            BrowserWindow browser = testInit();
            this.UIMapInvoices.invoicesLoginAction();
            this.UIMapInvoices.addInvoiceAction();
            List<string> details = this.UIMapInvoices.fillInvoiceToCheckDetails();
            this.UIMapInvoices.invoiceSubmitAction();
            this.UIMapInvoices.searchInvoiceAction(details[0]);
            this.UIMapInvoices.openInvoiceDetailsAction();
            this.UIMapInvoices.invoiceDetailsAssertion(details);
            //this.UIMapInvoices.logoutAction();
        }

        [TestMethod]
        public void TC_USER_INVOICES_007()
        {
            BrowserWindow browser = testInit();
            this.UIMapInvoices.invoicesLoginAction();
            this.UIMapInvoices.addInvoiceAction();
            string invNum = this.UIMapInvoices.fillInvoiceWithOneProduct();
            this.UIMapInvoices.invoiceSubmitAction();
            this.UIMapInvoices.searchInvoiceAction(invNum);
            this.UIMapInvoices.openInvoiceDetailsAction();
            this.UIMapInvoices.invoiceUnpaidStatusAssertion();
            this.UIMapInvoices.changeStatusAction();
            this.UIMapInvoices.searchInvoiceAction(invNum);
            this.UIMapInvoices.openInvoiceDetailsAction();
            this.UIMapInvoices.invoicePaidStatusAssertion();
            this.UIMapInvoices.logoutAction();
        }*/

        private BrowserWindow testInit()
        {
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            Playback.PlaybackSettings.MaximumRetryCount = 10;
            Playback.PlaybackSettings.ShouldSearchFailFast = false;
            Playback.PlaybackSettings.DelayBetweenActions = 500;
            Playback.PlaybackSettings.SearchTimeout = 1000;

            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            return BrowserWindow.Launch(new System.Uri("http://localhost:56133/auth/login?ReturnUrl=%2F"));
        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private TestContext testContextInstance;

        public UIMapInvoices UIMapInvoices
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMapInvoices();
                }

                return this.map;
            }
        }

        private UIMapInvoices map;
    }
}