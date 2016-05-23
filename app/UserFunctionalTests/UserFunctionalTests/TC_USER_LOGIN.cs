using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using UserFunctionalTests.UIMapLoginClasses;


namespace UserFunctionalTests
{
    /// <summary>
    /// Summary description for TC_USER_LOGIN
    /// </summary>
    [CodedUITest]
    public class TC_USER_LOGIN
    {
        public TC_USER_LOGIN()
        {
        }

        [TestMethod]
        public void TC_USER_LOGIN_001()
        {
            /** Poprawne logowanie użytkownika do Systemu Zarządzania Fakturami */
            BrowserWindow browser = testInit();
            this.UIMapLogin.loginAction();
            this.UIMapLogin.loginAssertions();
            this.UIMapLogin.loginSubmitAction();
            Assert.AreEqual("http://localhost:56133/", browser.Uri.ToString());
        }

        [TestMethod]
        public void TC_USER_LOGIN_002()
        {
            /** Niepoprawne logowanie użytkownika do Systemu Zarządzania Fakturami - błędny login lub hasło. */
            BrowserWindow browser = testInit();
            this.UIMapLogin.incorrectLoginAction();
            this.UIMapLogin.incorrectLoginAssertion();
        }

        [TestMethod]
        public void TC_USER_LOGIN_003()
        {
            /** Wylogowanie się użytkownika z Systemu Zarządzania Fakturami */
            BrowserWindow browser = testInit();
            this.UIMapLogin.loginAction();
            this.UIMapLogin.loginSubmitAction();
            this.UIMapLogin.logoutAction();
            this.UIMapLogin.logoutAssertion();

        }

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

        public UIMapLogin UIMapLogin
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMapLogin();
                }

                return this.map;
            }
        }

        private UIMapLogin map;
    }
}