﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 14.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace UserFunctionalTests.UIMapLoginClasses
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public partial class UIMapLogin
    {
        
        /// <summary>
        /// incorrectLoginAction - Use 'incorrectLoginActionParams' to pass parameters into this method.
        /// </summary>
        public void incorrectLoginAction()
        {
            #region Variable Declarations
            HtmlEdit uILoginEdit = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UILoginEdit;
            HtmlEdit uIPasswordEdit = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UIPasswordEdit;
            HtmlButton uILogInButton = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UILogInFormCustom.UILogInButton;
            #endregion

            // Type 'incorrectLogin' in 'login' text box
            uILoginEdit.Text = this.incorrectLoginActionParams.UILoginEditText;

            // Type '{Tab}' in 'login' text box
            Keyboard.SendKeys(uILoginEdit, this.incorrectLoginActionParams.UILoginEditSendKeys, ModifierKeys.None);

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.incorrectLoginActionParams.UIPasswordEditPassword;

            // Click 'Log In' button
            Mouse.Click(uILogInButton, new Point(26, 16));
        }
        
        /// <summary>
        /// incorrectLoginAssertion - Use 'incorrectLoginAssertionExpectedValues' to pass parameters into this method.
        /// </summary>
        public void incorrectLoginAssertion()
        {
            #region Variable Declarations
            HtmlDocument uILogowanieSystemzarząDocument = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument;
            #endregion

            StringAssert.Contains(uILogowanieSystemzarząDocument.InnerText, "Błędny login lub hasło");
        }

        public void logoutAssertion()
        {
            #region Variable Declarations
            HtmlDocument uILogowanieSystemzarząDocument = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument;
            #endregion

            StringAssert.Contains(uILogowanieSystemzarząDocument.InnerText, "Zaloguj");
        }

        /// <summary>
        /// loginAction - Use 'loginActionParams' to pass parameters into this method.
        /// </summary>
        public void loginAction()
        {
            #region Variable Declarations
            HtmlEdit uIEmailEdit = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UILoginEdit;
            HtmlEdit uIPasswordEdit = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UIPasswordEdit;
            HtmlButton uILogInButton = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UILogInFormCustom.UILogInButton;
            #endregion

            // Type 'admin@admin.com' in 'email' text box
            uIEmailEdit.Text = this.loginActionParams.UIEmailEditText;

            // Type '{Tab}' in 'email' text box
            Keyboard.SendKeys(uIEmailEdit, this.loginActionParams.UIEmailEditSendKeys, ModifierKeys.None);

            // Type '********' in 'Password' text box
            uIPasswordEdit.Password = this.loginActionParams.UIPasswordEditPassword;

            // Click 'Log In' button
            //Mouse.Click(uILogInButton, new Point(47, 11));
        }
        
        /// <summary>
        /// loginAssertions
        /// </summary>
        public void loginAssertions()
        {
            #region Variable Declarations
            HtmlEdit uIEmailEdit = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UILoginEdit;
            HtmlEdit uIPasswordEdit = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UIPasswordEdit;
            #endregion

            // Verify that the 'Text' property of 'email' text box is not equal to 'null'
            Assert.IsNotNull(uIEmailEdit.Text);

            // Verify that the 'InnerText' property of 'Password' text box is not equal to 'null'
            //Assert.IsNotNull(uIPasswordEdit.InnerText);
        }
        
        /// <summary>
        /// loginSubmitAction
        /// </summary>
        public void loginSubmitAction()
        {
            #region Variable Declarations
            HtmlButton uILogInButton = this.UILogowanieSystemzarząWindow.UILogowanieSystemzarząDocument.UILogInFormCustom.UILogInButton;
            #endregion

            // Click 'Log In' button
            Mouse.Click(uILogInButton, new Point(42, 15));
        }
        
        /// <summary>
        /// logoutAction
        /// </summary>
        public void logoutAction()
        {
            #region Variable Declarations
            HtmlHyperlink uIWylogujsięHyperlink = this.UIWyszukiwarkafakturSyWindow.UIWyszukiwarkafakturSyDocument.UIWylogujsięHyperlink;
            #endregion

            // Click 'Wyloguj się' link
            Mouse.Click(uIWylogujsięHyperlink, new Point(46, 29));
        }
        
        #region Properties
        public virtual incorrectLoginActionParams incorrectLoginActionParams
        {
            get
            {
                if ((this.mincorrectLoginActionParams == null))
                {
                    this.mincorrectLoginActionParams = new incorrectLoginActionParams();
                }
                return this.mincorrectLoginActionParams;
            }
        }
        
        public virtual incorrectLoginAssertionExpectedValues incorrectLoginAssertionExpectedValues
        {
            get
            {
                if ((this.mincorrectLoginAssertionExpectedValues == null))
                {
                    this.mincorrectLoginAssertionExpectedValues = new incorrectLoginAssertionExpectedValues();
                }
                return this.mincorrectLoginAssertionExpectedValues;
            }
        }
        
        public virtual loginActionParams loginActionParams
        {
            get
            {
                if ((this.mloginActionParams == null))
                {
                    this.mloginActionParams = new loginActionParams();
                }
                return this.mloginActionParams;
            }
        }
        
        public UILogowanieSystemzarząWindow UILogowanieSystemzarząWindow
        {
            get
            {
                if ((this.mUILogowanieSystemzarząWindow == null))
                {
                    this.mUILogowanieSystemzarząWindow = new UILogowanieSystemzarząWindow();
                }
                return this.mUILogowanieSystemzarząWindow;
            }
        }
        
        public UIWyszukiwarkafakturSyWindow UIWyszukiwarkafakturSyWindow
        {
            get
            {
                if ((this.mUIWyszukiwarkafakturSyWindow == null))
                {
                    this.mUIWyszukiwarkafakturSyWindow = new UIWyszukiwarkafakturSyWindow();
                }
                return this.mUIWyszukiwarkafakturSyWindow;
            }
        }
        #endregion
        
        #region Fields
        private incorrectLoginActionParams mincorrectLoginActionParams;
        
        private incorrectLoginAssertionExpectedValues mincorrectLoginAssertionExpectedValues;
        
        private loginActionParams mloginActionParams;
        
        private UILogowanieSystemzarząWindow mUILogowanieSystemzarząWindow;
        
        private UIWyszukiwarkafakturSyWindow mUIWyszukiwarkafakturSyWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'incorrectLoginAction'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class incorrectLoginActionParams
    {
        
        #region Fields
        /// <summary>
        /// Type 'incorrectLogin' in 'login' text box
        /// </summary>
        public string UILoginEditText = "incorrectLogin";
        
        /// <summary>
        /// Type '{Tab}' in 'login' text box
        /// </summary>
        public string UILoginEditSendKeys = "{Tab}";
        
        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "bbrr+jppkgLh8S6g5WOtNTY76V+qmxSex2z09ZjdSV8=";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'incorrectLoginAssertion'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class incorrectLoginAssertionExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that the 'InnerText' property of 'Logowanie - System zarządzania fakturami' document is not equal to 'Błędny email lub hasło.
        ///
        ///
        ///
        ///
        /// Faktury 
        ///
        ///Rejestracja
        ///
        ///
        ///
        ///Zaloguj się
        ///
        ///Błędny email lub hasło.
        ///
        ///
        ///Login:   
        ///Hasło:   
        ///
        ///
        ///Log In 
        ///
        ///
        ///© 2016 - System zarządzania fakturami
        ///    {"appName":"Internet Explorer","requestId":"edffeba8f8f84756aa1555a98b1cb2ba"}    '
        /// </summary>
        public string UILogowanieSystemzarząDocumentInnerText = @"Błędny login lub hasło.




 Faktury 

Rejestracja



Zaloguj się

Błędny email lub hasło.


Login:   
Hasło:   


Log In 


© 2016 - System zarządzania fakturami
    {""appName"":""Internet Explorer"",""requestId"":""edffeba8f8f84756aa1555a98b1cb2ba""}    ";
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'loginAction'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class loginActionParams
    {
        
        #region Fields
        /// <summary>
        /// Type 'admin@admin.com' in 'email' text box
        /// </summary>
        public string UIEmailEditText = "admin@admin.com";
        
        /// <summary>
        /// Type '{Tab}' in 'email' text box
        /// </summary>
        public string UIEmailEditSendKeys = "{Tab}";
        
        /// <summary>
        /// Type '********' in 'Password' text box
        /// </summary>
        public string UIPasswordEditPassword = "QWwOodIXpSM441BCNCWNvH9k+8fhegdI";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UILogowanieSystemzarząWindow : BrowserWindow
    {
        
        public UILogowanieSystemzarząWindow()
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.Name] = "Logowanie - System zarządzania fakturami";
            this.SearchProperties[UITestControl.PropertyNames.ClassName] = "IEFrame";
            this.WindowTitles.Add("Logowanie - System zarządzania fakturami");
            #endregion
        }
        
        public void LaunchUrl(System.Uri url)
        {
            this.CopyFrom(BrowserWindow.Launch(url));
        }
        
        #region Properties
        public UILogowanieSystemzarząDocument UILogowanieSystemzarząDocument
        {
            get
            {
                if ((this.mUILogowanieSystemzarząDocument == null))
                {
                    this.mUILogowanieSystemzarząDocument = new UILogowanieSystemzarząDocument(this);
                }
                return this.mUILogowanieSystemzarząDocument;
            }
        }
        #endregion
        
        #region Fields
        private UILogowanieSystemzarząDocument mUILogowanieSystemzarząDocument;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UILogowanieSystemzarząDocument : HtmlDocument
    {
        
        public UILogowanieSystemzarząDocument(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[HtmlDocument.PropertyNames.Id] = null;
            this.SearchProperties[HtmlDocument.PropertyNames.RedirectingPage] = "False";
            this.SearchProperties[HtmlDocument.PropertyNames.FrameDocument] = "False";
            this.FilterProperties[HtmlDocument.PropertyNames.Title] = "Logowanie - System zarządzania fakturami";
            this.FilterProperties[HtmlDocument.PropertyNames.AbsolutePath] = "/auth/login";
            this.FilterProperties[HtmlDocument.PropertyNames.PageUrl] = "http://localhost:56133/auth/login?ReturnUrl=%2F";
            this.WindowTitles.Add("Logowanie - System zarządzania fakturami");
            #endregion
        }
        
        #region Properties
        public HtmlEdit UIEmailEdit
        {
            get
            {
                if ((this.mUIEmailEdit == null))
                {
                    this.mUIEmailEdit = new HtmlEdit(this);
                    #region Search Criteria
                    this.mUIEmailEdit.SearchProperties[HtmlEdit.PropertyNames.Id] = "email";
                    this.mUIEmailEdit.SearchProperties[HtmlEdit.PropertyNames.Name] = "email";
                    this.mUIEmailEdit.FilterProperties[HtmlEdit.PropertyNames.LabeledBy] = null;
                    this.mUIEmailEdit.FilterProperties[HtmlEdit.PropertyNames.Type] = "SINGLELINE";
                    this.mUIEmailEdit.FilterProperties[HtmlEdit.PropertyNames.Title] = null;
                    this.mUIEmailEdit.FilterProperties[HtmlEdit.PropertyNames.Class] = "input-sm form-control margin-right-10 margin-left-20";
                    this.mUIEmailEdit.FilterProperties[HtmlEdit.PropertyNames.ControlDefinition] = "name=\"email\" class=\"input-sm form-contro";
                    this.mUIEmailEdit.FilterProperties[HtmlEdit.PropertyNames.TagInstance] = "1";
                    this.mUIEmailEdit.WindowTitles.Add("Logowanie - System zarządzania fakturami");
                    #endregion
                }
                return this.mUIEmailEdit;
            }
        }
        
        public HtmlEdit UIPasswordEdit
        {
            get
            {
                if ((this.mUIPasswordEdit == null))
                {
                    this.mUIPasswordEdit = new HtmlEdit(this);
                    #region Search Criteria
                    this.mUIPasswordEdit.SearchProperties[HtmlEdit.PropertyNames.Id] = "Password";
                    this.mUIPasswordEdit.SearchProperties[HtmlEdit.PropertyNames.Name] = "Password";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.LabeledBy] = null;
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.Type] = "PASSWORD";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.Title] = null;
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.Class] = "input-sm form-control margin-right-10 margin-left-20";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.ControlDefinition] = "name=\"Password\" class=\"input-sm form-con";
                    this.mUIPasswordEdit.FilterProperties[HtmlEdit.PropertyNames.TagInstance] = "2";
                    this.mUIPasswordEdit.WindowTitles.Add("Logowanie - System zarządzania fakturami");
                    #endregion
                }
                return this.mUIPasswordEdit;
            }
        }
        
        public UILogInFormCustom UILogInFormCustom
        {
            get
            {
                if ((this.mUILogInFormCustom == null))
                {
                    this.mUILogInFormCustom = new UILogInFormCustom(this);
                }
                return this.mUILogInFormCustom;
            }
        }
        
        public HtmlEdit UILoginEdit
        {
            get
            {
                if ((this.mUILoginEdit == null))
                {
                    this.mUILoginEdit = new HtmlEdit(this);
                    #region Search Criteria
                    this.mUILoginEdit.SearchProperties[HtmlEdit.PropertyNames.Id] = "login";
                    this.mUILoginEdit.SearchProperties[HtmlEdit.PropertyNames.Name] = "login";
                    this.mUILoginEdit.FilterProperties[HtmlEdit.PropertyNames.LabeledBy] = null;
                    this.mUILoginEdit.FilterProperties[HtmlEdit.PropertyNames.Type] = "SINGLELINE";
                    this.mUILoginEdit.FilterProperties[HtmlEdit.PropertyNames.Title] = null;
                    this.mUILoginEdit.FilterProperties[HtmlEdit.PropertyNames.Class] = "input-sm form-control margin-right-10 margin-left-20";
                    this.mUILoginEdit.FilterProperties[HtmlEdit.PropertyNames.ControlDefinition] = "name=\"login\" class=\"input-sm form-contro";
                    this.mUILoginEdit.FilterProperties[HtmlEdit.PropertyNames.TagInstance] = "1";
                    this.mUILoginEdit.WindowTitles.Add("Logowanie - System zarządzania fakturami");
                    #endregion
                }
                return this.mUILoginEdit;
            }
        }
        #endregion
        
        #region Fields
        private HtmlEdit mUIEmailEdit;
        
        private HtmlEdit mUIPasswordEdit;
        
        private UILogInFormCustom mUILogInFormCustom;
        
        private HtmlEdit mUILoginEdit;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UILogInFormCustom : HtmlCustom
    {
        
        public UILogInFormCustom(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties["TagName"] = "FORM";
            this.SearchProperties["Id"] = "logInForm";
            this.SearchProperties[UITestControl.PropertyNames.Name] = null;
            this.FilterProperties["Class"] = "form-inline";
            this.FilterProperties["ControlDefinition"] = "class=\"form-inline\" id=\"logInForm\" actio";
            this.FilterProperties["TagInstance"] = "1";
            this.WindowTitles.Add("Logowanie - System zarządzania fakturami");
            #endregion
        }
        
        #region Properties
        public HtmlButton UILogInButton
        {
            get
            {
                if ((this.mUILogInButton == null))
                {
                    this.mUILogInButton = new HtmlButton(this);
                    #region Search Criteria
                    this.mUILogInButton.SearchProperties[HtmlButton.PropertyNames.Id] = null;
                    this.mUILogInButton.SearchProperties[HtmlButton.PropertyNames.Name] = null;
                    this.mUILogInButton.SearchProperties[HtmlButton.PropertyNames.DisplayText] = "Log In";
                    this.mUILogInButton.SearchProperties[HtmlButton.PropertyNames.Type] = "submit";
                    this.mUILogInButton.FilterProperties[HtmlButton.PropertyNames.Title] = null;
                    this.mUILogInButton.FilterProperties[HtmlButton.PropertyNames.Class] = "btn margin-top-10 margin-bottom-10";
                    this.mUILogInButton.FilterProperties[HtmlButton.PropertyNames.ControlDefinition] = "class=\"btn margin-top-10 margin-bottom-1";
                    this.mUILogInButton.FilterProperties[HtmlButton.PropertyNames.TagInstance] = "1";
                    this.mUILogInButton.WindowTitles.Add("Logowanie - System zarządzania fakturami");
                    #endregion
                }
                return this.mUILogInButton;
            }
        }
        #endregion
        
        #region Fields
        private HtmlButton mUILogInButton;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIWyszukiwarkafakturSyWindow : BrowserWindow
    {
        
        public UIWyszukiwarkafakturSyWindow()
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.Name] = "Wyszukiwarka faktur - System zarządzania fakturami";
            this.SearchProperties[UITestControl.PropertyNames.ClassName] = "IEFrame";
            this.WindowTitles.Add("Wyszukiwarka faktur - System zarządzania fakturami");
            #endregion
        }
        
        public void LaunchUrl(System.Uri url)
        {
            this.CopyFrom(BrowserWindow.Launch(url));
        }
        
        #region Properties
        public UIWyszukiwarkafakturSyDocument UIWyszukiwarkafakturSyDocument
        {
            get
            {
                if ((this.mUIWyszukiwarkafakturSyDocument == null))
                {
                    this.mUIWyszukiwarkafakturSyDocument = new UIWyszukiwarkafakturSyDocument(this);
                }
                return this.mUIWyszukiwarkafakturSyDocument;
            }
        }
        #endregion
        
        #region Fields
        private UIWyszukiwarkafakturSyDocument mUIWyszukiwarkafakturSyDocument;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class UIWyszukiwarkafakturSyDocument : HtmlDocument
    {
        
        public UIWyszukiwarkafakturSyDocument(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[HtmlDocument.PropertyNames.Id] = null;
            this.SearchProperties[HtmlDocument.PropertyNames.RedirectingPage] = "False";
            this.SearchProperties[HtmlDocument.PropertyNames.FrameDocument] = "False";
            this.FilterProperties[HtmlDocument.PropertyNames.Title] = "Wyszukiwarka faktur - System zarządzania fakturami";
            this.FilterProperties[HtmlDocument.PropertyNames.AbsolutePath] = "/";
            this.FilterProperties[HtmlDocument.PropertyNames.PageUrl] = "http://localhost:56133/";
            this.WindowTitles.Add("Wyszukiwarka faktur - System zarządzania fakturami");
            #endregion
        }
        
        #region Properties
        public HtmlHyperlink UIWylogujsięHyperlink
        {
            get
            {
                if ((this.mUIWylogujsięHyperlink == null))
                {
                    this.mUIWylogujsięHyperlink = new HtmlHyperlink(this);
                    #region Search Criteria
                    this.mUIWylogujsięHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Id] = null;
                    this.mUIWylogujsięHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Name] = null;
                    this.mUIWylogujsięHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.Target] = null;
                    this.mUIWylogujsięHyperlink.SearchProperties[HtmlHyperlink.PropertyNames.InnerText] = "Wyloguj się";
                    this.mUIWylogujsięHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.AbsolutePath] = "/Auth/Logout";
                    this.mUIWylogujsięHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Title] = null;
                    this.mUIWylogujsięHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Href] = "http://localhost:56133/Auth/Logout";
                    this.mUIWylogujsięHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.Class] = null;
                    this.mUIWylogujsięHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.ControlDefinition] = "href=\"/Auth/Logout\"";
                    this.mUIWylogujsięHyperlink.FilterProperties[HtmlHyperlink.PropertyNames.TagInstance] = "5";
                    this.mUIWylogujsięHyperlink.WindowTitles.Add("Wyszukiwarka faktur - System zarządzania fakturami");
                    #endregion
                }
                return this.mUIWylogujsięHyperlink;
            }
        }
        #endregion
        
        #region Fields
        private HtmlHyperlink mUIWylogujsięHyperlink;
        #endregion
    }
}
