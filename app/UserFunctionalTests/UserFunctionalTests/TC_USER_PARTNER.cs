using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace UserFunctionalTests
{
    /// <summary>
    /// Summary description for TC_USER_PARTNER
    /// </summary>
    [CodedUITest]
    public class TC_USER_PARTNER
    {
        public TC_USER_PARTNER()
        {
        }

        [TestMethod]
        public void TC_USER_PARTNER_001()
        {
            /** Dodanie partnera biznesowego */
            BrowserWindow browser = testInit();

            string vatin = this.UIMap.fillAddPartnerParameters();
            this.UIMap.addPartnerParameterAssertions();
            this.UIMap.addPartnerSubmitAction();
            Assert.AreEqual("http://localhost:56133/", browser.Uri.ToString());
            this.UIMap.listPartnersAction();
            this.UIMap.vatinAssertion(vatin);
        }

        [TestMethod]
        public void TC_USER_PARTNER_002()
        {
            /** Partner biznesowy już istnieje */
            BrowserWindow browser = testInit();

            string vatin = this.UIMap.fillAddPartnerParameters();
            this.UIMap.addPartnerSubmitAction();
            this.UIMap.fillTheSecondPartner(vatin);
            this.UIMap.addPartnerSubmitAction();
        }

        [TestMethod]
        public void TC_USER_PARTNER_003()
        {
            /** Test widoku wyświetlającego partnerów biznesowych zalogowanego użytkownika. */
            BrowserWindow browser = testInit();

            string v1 = this.UIMap.addPartnersToSearch();
            this.UIMap.addPartnerSubmitAction();
            string v2 = this.UIMap.addSecondPartnerToSearch();
            this.UIMap.addPartnerSubmitAction();
            this.UIMap.findPartnersInTheSameCompanyAction();
            this.UIMap.companyAssertion(v1, v2);
        }


        private BrowserWindow testInit()
        {
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            Playback.PlaybackSettings.MaximumRetryCount = 10;
            Playback.PlaybackSettings.ShouldSearchFailFast = false;
            Playback.PlaybackSettings.DelayBetweenActions = 500;
            Playback.PlaybackSettings.SearchTimeout = 1000;

            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            return BrowserWindow.Launch(new System.Uri("http://localhost:56133/"));
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

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}