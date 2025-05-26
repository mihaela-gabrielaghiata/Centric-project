using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;

namespace Proiect
{
    [TestClass]
    public class PurchaseFlowTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [TestInitialize]
        public void SetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [TestCleanup]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestMethod]
        public void SearchAndAddProductToCart()
        {
            driver.Navigate().GoToUrl("https://www.optimusdigital.ro/");
            Console.WriteLine(" Navigat la site.");

            
            try
            {
                var acceptCookies = wait.Until(d => d.FindElement(By.Id("lgcookieslaw_accept")));
                if (acceptCookies.Displayed && acceptCookies.Enabled)
                {
                    acceptCookies.Click();
                    Console.WriteLine(" Accept cookies apăsat.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine(" Butonul de cookies NU a apărut.");
            }

            
            var searchInput = wait.Until(d => d.FindElement(By.Name("search_query")));
            searchInput.SendKeys("arduino");
            searchInput.SendKeys(Keys.Enter);
            Console.WriteLine(" Căutare 'arduino' efectuată.");

            wait.Until(d => d.FindElements(By.CssSelector(".product_list .product-container .product-name")).Count > 0);
            var products = driver.FindElements(By.CssSelector(".product_list .product-container .product-name"));

            IWebElement targetProduct = products.FirstOrDefault(p => p.Text.Contains("Arduino MEGA 2560"));
            Assert.IsNotNull(targetProduct, " Produsul 'Arduino MEGA 2560' NU a fost găsit.");
            Console.WriteLine(" Produsul a fost găsit.");

            targetProduct.Click();

            var addToCart = wait.Until(d => d.FindElement(By.Id("add_to_cart")));
            Assert.IsTrue(addToCart.Displayed, " Butonul 'Adaugă în coș' NU este vizibil.");
            addToCart.Click();
            Console.WriteLine(" Click pe 'Adaugă în coș'.");

            
            var requestOrder = wait.Until(d =>
            {
                var buttons = d.FindElements(By.CssSelector("a.btn"));
                foreach (var btn in buttons)
                {
                    if (btn.Text.Trim().Contains("Solicitare comandă"))
                        return btn;
                }
                throw new NoSuchElementException("Butonul 'Solicitare comandă' NU a fost găsit.");
            });
            requestOrder.Click();
            Console.WriteLine(" Click pe 'Solicitare comandă'.");

            
            var cartItems = wait.Until(d => d.FindElements(By.CssSelector(".cart_description .product-name")));
            bool found = cartItems.Any(item => item.Text.Contains("Arduino MEGA 2560"));
            Assert.IsTrue(found, " Produsul NU este prezent în coș.");
            Console.WriteLine("Produsul se află în coș.");
        }
    }
}
