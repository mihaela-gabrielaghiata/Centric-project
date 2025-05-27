using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectCentric
{
    public class AddAndRemoveFromCart
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private Actions actions;

        public AddAndRemoveFromCart(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
            this.actions = new Actions(driver);
        }

        public void AddAndRemoveHeadphones(string productName)
        {
            WaitForCookies();
            HoverAndClickHeadphones();
            VerifyProductList();
            SelectAndAddSpecificProduct(productName);
            GoToCart();
            RemoveProductFromCart(productName);
        }

        private void WaitForCookies()
        {
            try
            {
                var acceptCookies = wait.Until(d => d.FindElement(By.Id("lgcookieslaw_accept")));
                if (acceptCookies.Displayed && acceptCookies.Enabled)
                {
                    acceptCookies.Click();
                    Console.WriteLine("Accept cookies apăsat.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Butonul de cookies NU a apărut.");
            }
        }

        private void HoverAndClickHeadphones()
        {
            var produseMenu = wait.Until(d => d.FindElement(By.XPath("//*[@id=\"block_top_menu\"]/ul/li[1]/a")));
            actions.MoveToElement(produseMenu).Perform();

            var castiLink = wait.Until(d => d.FindElement(By.LinkText("Căști")));
            castiLink.Click();
            Assert.IsTrue(driver.FindElement(By.XPath("//*[@id=\"center_column\"]/h1/span[1]")).Text.Contains("CĂȘTI"));
            Console.WriteLine("Navigat la categoria Căști.");
        }

        private void VerifyProductList()
        {
            wait.Until(d => d.FindElements(By.CssSelector(".product_list .product-container .product-name")).Count > 0);
            var productList = wait.Until(d => d.FindElements(By.CssSelector(".product_list .product-container .product-name")));
            Assert.IsTrue(productList.Count > 0, "Lista de produse este goală.");
            Console.WriteLine("Produse găsite în listă.");
        }

        private void SelectAndAddSpecificProduct(string productName = "Căști Metalice cu Microfon \"London\"")
        {
            var products = driver.FindElements(By.CssSelector(".product_list .product-container .product-name"));
            var targetProduct = products.FirstOrDefault(p => p.Text.Contains(productName));

            Assert.IsNotNull(targetProduct, "Produsul specific NU a fost găsit.");
            targetProduct.Click();

            var addToCart = wait.Until(d => d.FindElement(By.Id("add_to_cart")));
            Assert.IsTrue(addToCart.Displayed, "Butonul 'Cumpără' NU este vizibil.");
            addToCart.Click();

            Console.WriteLine("Produs adăugat în coș.");
        }

        private void GoToCart()
        {
            wait.Until(d =>
            {
                var qtySpan = d.FindElement(By.ClassName("ajax_cart_quantity"));
                return qtySpan.Text == "1";
            });
            Console.WriteLine("Produsul a fost adăugat în coș (cantitate 1).");

            try
            {
                wait.Until(d => d.FindElement(By.CssSelector("span.cross")));
                actions.MoveByOffset(15, 15).Click().Perform();
                Console.WriteLine("Pop-up închis.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Pop-up-ul nu a apărut.");
            }

            var cos = driver.FindElement(By.XPath("//*[@id='header']/div[2]/div/div[2]/div[3]/div/a"));
            cos.Click();

            Assert.IsTrue(driver.FindElement(By.Id("cart_title")).Text.Contains("SUMARUL"));

            Console.WriteLine("Navigat în coș.");
        }

        private void RemoveProductFromCart(string productName = "Căști Metalice cu Microfon \"London\"")
        {
            
            var deleteIcon = wait.Until(d => d.FindElement(By.ClassName("cart_quantity_delete")));
            deleteIcon.Click();
            var emptyCartMessage = wait.Until(d =>
            {
                var elem = d.FindElement(By.CssSelector("p.alert.alert-warning"));
                if (!string.IsNullOrEmpty(elem.Text) && elem.Text.Contains("gol"))
                    return elem;
                return null;
            });

            Assert.IsTrue(emptyCartMessage.Text.Contains("gol"));
            Console.WriteLine("Produsul a fost eliminat din coș.");
        }
    }
}
