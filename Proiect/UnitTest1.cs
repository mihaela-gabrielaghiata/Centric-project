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

        [TestInitialize]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            addToCart = new AddToCart(driver, wait);

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
            string keyWord = "arduino";
            string desiredProduct = "Arduino MEGA 2560";
            this.addToCart.SearchAndAddProductToCart(keyWord, desiredProduct);
        }

    }
}
