using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace ProiectCentric
{
    [TestClass]
    public class PurchaseFlowTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private AddToCart addToCart;
        private LogInAndLogOut logInAndOut;
        private AddAndRemoveFromCart addAndRemove;
        private ChangeLangCurrencyAndReadReviews changeLangCurrencyAndReadReviews;

        [TestInitialize]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            addToCart = new AddToCart(driver, wait);
            logInAndOut = new LogInAndLogOut(driver, wait);
            addAndRemove = new AddAndRemoveFromCart(driver, wait);
            changeLangCurrencyAndReadReviews = new ChangeLangCurrencyAndReadReviews(driver, wait);

            driver.Navigate().GoToUrl("https://www.optimusdigital.ro/");
            Console.WriteLine(" Navigat la site.");
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestMethod]
        public void SearchAndAddProductToCart()
        {
            // folosim searchbar
            string keyWord = "arduino";
            string desiredProduct = "Arduino MEGA 2560";
            this.addToCart.SearchAndAddProductToCart(keyWord, desiredProduct);
        }
        [TestMethod]
        public void LogInAndOut()
        {
            this.logInAndOut.LogInAndOut();
        }
        [TestMethod]
        public void AddAndRemoveCart()
        {
            // dam hover peste produse -> casti && inchidem popup cu un click
            string productName = "Căști Metalice cu Microfon \"London\"";
            this.addAndRemove.AddAndRemoveHeadphones(productName);
        }
        [TestMethod]
        public void ReadReviews()
        {
            string productName = "Raspberry Pi 4 Model B/4GB";
            this.changeLangCurrencyAndReadReviews.ReadReview(productName);
        }

    }
}
