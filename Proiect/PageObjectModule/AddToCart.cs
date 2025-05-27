using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ProiectCentric
{
    public class AddToCart
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public AddToCart(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
        }
        public void SearchAndAddProductToCart(string keyword = "arduino", string desiredProduct = "Arduino MEGA 2560")
        {
            this.WaitForCookies();

            this.SearchProduct(keyword);

            this.SelectProductFromResults(desiredProduct);

            this.AddProductToCart();

            this.ClickRequestOrderButton();

            this.VerifyProductInCart();
        }
        private void WaitForCookies()
        {
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
        }
        private void SearchProduct(string keyword)
        {
            var searchInput = wait.Until(d => d.FindElement(By.Name("search_query")));
            searchInput.SendKeys(keyword);
            searchInput.SendKeys(Keys.Enter);
            Console.WriteLine($" Căutare '{keyword}' efectuată.");
        }
        private void SelectProductFromResults(string desiredProduct)
        {
            wait.Until(d => d.FindElements(By.CssSelector(".product_list .product-container .product-name")).Count > 0);
            var products = driver.FindElements(By.CssSelector(".product_list .product-container .product-name"));

            IWebElement targetProduct = products.FirstOrDefault(p => p.Text.Contains(desiredProduct));
            Assert.IsNotNull(targetProduct, $" Produsul '{desiredProduct}' NU a fost găsit.");
            Console.WriteLine(" Produsul a fost găsit.");

            targetProduct.Click();
        }
        private void AddProductToCart()
        {
            var addToCart = wait.Until(d => d.FindElement(By.Id("add_to_cart")));
            Assert.IsTrue(addToCart.Displayed, " Butonul 'Adaugă în coș' NU este vizibil.");
            addToCart.Click();
            Console.WriteLine(" Click pe 'Adaugă în coș'.");
        }
        private void ClickRequestOrderButton()
        {
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
        }
        private void  VerifyProductInCart()
            {
                var cartItems = wait.Until(d => d.FindElements(By.CssSelector(".cart_description .product-name")));
                bool found = cartItems.Any(item => item.Text.Contains("Arduino MEGA 2560"));
                Assert.IsTrue(found, " Produsul NU este prezent în coș.");
                Console.WriteLine("Produsul se află în coș.");
            }

    }
}
