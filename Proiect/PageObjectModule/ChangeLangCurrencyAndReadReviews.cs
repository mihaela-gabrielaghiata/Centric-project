using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Interactions;

namespace ProiectCentric
{
    public class ChangeLangCurrencyAndReadReviews
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private Actions actions;
        public ChangeLangCurrencyAndReadReviews(IWebDriver driver, WebDriverWait wait)
        {
            this.driver = driver;
            this.wait = wait;
            this.actions = new Actions(driver);
        }

        public void ReadReview(string productName = "Raspberry Pi 4 Model B/4G")
        {
            this.WaitForCookies();
            this.ChangeLang();
            this.SearchProduct(productName);
            this.SelectProductFromResults(productName);
            this.ChangeCurrency();
            this.AccessReviews();
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
        private void ChangeLang()
        {
            var currentLangDiv = wait.Until(d => d.FindElement(By.CssSelector("div#languages-block-top")));
            actions.MoveToElement(currentLangDiv).Click().Perform();

            var engLang = wait.Until(d => d.FindElement(By.CssSelector("div#languages-block-top ul#first-languages a")));
            wait.Until(d => engLang.Displayed && engLang.Enabled);
            actions.MoveToElement(engLang).Click().Perform();

            Assert.IsTrue(wait.Until(d => d.FindElement(By.CssSelector("div#contact-link a"))).Text.Contains("Contact us"));
            Console.WriteLine("Limba a fost schimbată în engleză.");
        }
        private void ChangeCurrency()
        {
            var currentCurrDiv = wait.Until(d => d.FindElement(By.Id("setCurrency")));
            actions.MoveToElement(currentCurrDiv).Click().Perform();
            
            var USDcurr = wait.Until(d => d.FindElement(By.XPath("//*[@id='first-currencies']/li[3]/a")));
            wait.Until(d => USDcurr.Displayed && USDcurr.Enabled);
            actions.MoveToElement(USDcurr).Click().Perform();
            var USD_Verify = wait.Until(d =>
            {
                var elem = d.FindElement(By.CssSelector("div span#our_price_display"));
                return elem.Displayed && elem.Text.Contains("$") ? elem : null;
            });
            Assert.IsTrue(USD_Verify.Text.Contains("$"));
            Console.WriteLine("Moneda a fost schimbată în USD.");
        }
        private void SearchProduct(string keyword)
        {
            var searchInput = wait.Until(d => d.FindElement(By.Name("search_query")));
            searchInput.SendKeys(keyword);
            searchInput.SendKeys(Keys.Enter);
            Assert.IsTrue(driver.FindElement(By.XPath("//*[@id=\"center_column\"]/h1/span[1]")).Text.Contains("RASPBERRY"));
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
        private void AccessReviews()
        {
            var reviewsLink = driver.FindElement(By.CssSelector("a.reviews"));
            reviewsLink.Click();

            wait.Until(d => d.FindElements(By.CssSelector("div.comment_author_infos strong")).Count > 0);
            var allAuthors = driver.FindElements(By.CssSelector("div.comment_author_infos strong"));
            var foundAuthor = allAuthors.FirstOrDefault(e => e.Displayed && e.Text.Contains("Alexandru"));

            if (foundAuthor != null)
            {
                Console.WriteLine($"Autor găsit: {foundAuthor.Text}");
                Console.WriteLine("S-a accesat link-ul către review-uri. Se pot citi review-urile.");
            }
            else
            {
                Console.WriteLine("Nu s-a găsit niciun review de la Alexandru.");
                Assert.Fail("Review-ul de la Alexandru nu a fost găsit.");
            }
        }
    }
}
