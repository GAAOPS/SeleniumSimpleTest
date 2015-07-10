using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumSimpleTest
{
    [TestClass]
    public class WikiTest
    {
        private IWebDriver _driver = null;
        private string _query = String.Empty;
        private string _titleElementId;

        /// <summary>
        /// Initializes driver and navigate to destination.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            _driver.Navigate().GoToUrl("https://en.wikipedia.org/wiki/Main_Page");
            _query = "Albert Einstein";
            _titleElementId = "firstHeading";
        }

        /// <summary>
        /// Close the browser instances and cleans up.
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [TestMethod]
        public void Bio_Of_Einstein_Can_Be_Found()
        {
            DoSearch();
            IWebElement titleElement = FindElementById(_titleElementId);
            Assert.AreEqual(_query, titleElement.Text);
        }

        [TestMethod]
        public void German_Translated_Version_Exist()
        {
            // coming to Einstein English Bio Page
            DoSearch();   
            // Getting German Link of Bio
            GetPageByLanguage("de");
            IWebElement titleElement = FindElementById(_titleElementId);
            Assert.AreEqual("de", titleElement.GetAttribute("lang"));
        }

        #region Helper Methods

        private IWebElement FindElementById(string idToFind)
        {
            return _driver.FindElement(By.Id(idToFind));
        }

        private void DoSearch()
        {
            IWebElement searchBoxElement = _driver.FindElement(By.Id("searchInput"));
            searchBoxElement.SendKeys(_query);
            searchBoxElement.SendKeys(Keys.Enter);
        }

        private void GetPageByLanguage(string language)
        {
            IWebElement germanLink =
                _driver.FindElement(By.CssSelector(string.Format("a[lang='{0}'][hreflang='{0}']", language)));
            germanLink.Click();
        }

        #endregion
    }
}
