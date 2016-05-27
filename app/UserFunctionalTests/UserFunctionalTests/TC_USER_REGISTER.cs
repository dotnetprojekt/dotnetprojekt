using System;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using UserFunctionalTests.UIMapRegistrationClasses;


namespace UserFunctionalTests
{
    /// <summary>
    /// Summary description for TC_USER_REGISTER
    /// </summary>
    [CodedUITest]
    public class TC_USER_REGISTER
    {
        public TC_USER_REGISTER()
        {
        }

        [TestMethod]
        public void TC_USER_REGISTER_001()
        {
            /** Poprawna i kompletna rejestracja użytkownika do Systemu Zarządzania Fakturami **/
            BrowserWindow browser = testInit();
            this.UIMapRegistration.clickRegisterAction();
            string uniqueNum = new Random().Next(100000000).ToString();
            this.UIMapRegistration.fillRegisterFormAction(uniqueNum);
            this.UIMapRegistration.registerAssertions();
            this.UIMapRegistration.registerSubmitAction();
            //this.UIMapRegistration.loginWithCreatedUserAction(uniqueNum);
            //Assert.AreEqual("http://localhost:56133/", browser.Uri.ToString());
        }

        /*[TestMethod]
        public void TC_USER_REGISTER_002()
        {
            BrowserWindow browser = testInit();
            this.UIMapRegistration.clickRegisterAction();
            string uniqueNum = new Random().Next(100000000).ToString();
            this.UIMapRegistration.fillRegisterFormAction(uniqueNum);
            this.UIMapRegistration.registerSubmitAction();
            this.UIMapRegistration.clickRegisterAction();
            this.UIMapRegistration.fillRegisterFormAction(uniqueNum);
            this.UIMapRegistration.registerSubmitAction();
            this.UIMapRegistration.incorrectUserAssertion();
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
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMapRegistration UIMapRegistration
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMapRegistration();
                }

                return this.map;
            }
        }

        private UIMapRegistration map;
    }
}
