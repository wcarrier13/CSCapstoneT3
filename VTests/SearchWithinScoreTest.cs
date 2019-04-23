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
    //Tests that the search functionality works within the score page
    [TestFixture]
    public class SearchWithinScoreTest
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            //Individuals will need to change this
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
        public void TheSearchWithinScoreTest()
        {
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/Score");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).Clear();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).SendKeys("The Ndw World");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).SendKeys(Keys.Enter);
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).Clear();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).SendKeys("The New World");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Score'])[1]/following::input[1]")).SendKeys(Keys.Enter);
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
