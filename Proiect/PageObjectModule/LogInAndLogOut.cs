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
    public class LogInAndLogOut
    {
        private const string PASS = "Parolalogare123";
        private const string EMAIL = "ivlad133@yahoo.com";
        private IWebDriver driver;
        private WebDriverWait wait;
        public LogInAndLogOut(IWebDriver driver, WebDriverWait wait) {
            this.driver = driver;
            this.wait = wait;
        }
        public void LogInAndOut()
        {
            this.WaitForCookies();
            this.LogIn();
            this.LogOut();
        }

        private void LogIn()
        {
            var signInLink = wait.Until(d => d.FindElement(By.LinkText("Autentificare")));
            signInLink.Click();
            Console.WriteLine("Încercare de logare.");

            var emailInput = wait.Until(d => d.FindElement(By.Id("email")));
            emailInput.SendKeys(EMAIL);

            var passwordInput = driver.FindElement(By.Id("passwd"));
            passwordInput.SendKeys(PASS);

            var submitBtn = driver.FindElement(By.Id("SubmitLogin"));
            submitBtn.Click();

            wait.Until(d => d.FindElement(By.LinkText("Deconectare")));

            Assert.IsTrue(driver.FindElement(By.ClassName("logout")).Text.Contains("Deconectare"));
            Console.WriteLine("Logare realizată cu succes.");
        }
        private void LogOut()
        {
            var logoutLink = wait.Until(d => d.FindElement(By.ClassName("logout")));
            logoutLink.Click();

            wait.Until(d => d.FindElement(By.LinkText("Autentificare")));
            Assert.IsTrue(driver.FindElement(By.ClassName("login")).Text.Contains("Autentificare"));

            Console.WriteLine("Delogare realizată cu succes.");
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
    }
}
