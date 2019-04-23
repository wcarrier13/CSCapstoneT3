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

namespace SeleniumTests
{
    //Checks to see if we can check scores in
    [TestFixture]
    public class CheckInScoresTest
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;

        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
            //Individual users will need to change this property
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
        public void TheCheckInScoresTest()
        {
            //New tests will have to change the values of the scores
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/CheckIn/Score");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Nebraska'])[1]/following::a[1]")).Click();
            driver.FindElement(By.Name("Checkin 134 16")).Click();
            driver.FindElement(By.Id("condition")).Click();
            new SelectElement(driver.FindElement(By.Id("condition"))).SelectByText("Excellent");
            driver.FindElement(By.Id("condition")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Clarinet 1'])[1]/following::input[1]")).Click();
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
