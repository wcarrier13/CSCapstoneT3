using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Chrome;



namespace Lizst
{
    //Tests to make sure the genre sidebar buttons work
    [TestFixture]
    public class GenreButtonTest
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            //Individual users will need to change this
            driver = new ChromeDriver(@"C:\Users\mchry\Downloads\chromedriver_win32");
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheGenreButtonTest()
        {
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Checked Out Pieces'])[1]/following::span[1]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.LinkText("Genres")).Click();
            driver.FindElement(By.LinkText("Medieval")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Checked Out Pieces'])[1]/following::span[1]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.LinkText("Genres")).Click();
            driver.FindElement(By.LinkText("Romantic")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Checked Out Pieces'])[1]/following::span[1]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.LinkText("Genres")).Click();
            driver.FindElement(By.LinkText("Baroque")).Click();
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
