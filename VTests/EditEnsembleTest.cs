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
    //Tests that an ensemble edits properly
    [TestFixture]
    public class EditEnsembleTest
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
        public void TheEditEnsembleTest()
        {
            //New tests will have to change individual values for the ensemble
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/Ensemble");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Matthew Brooks'])[1]/following::a[1]")).Click();
            driver.FindElement(By.Id("EnsembleName")).Click();
            driver.FindElement(By.Id("EnsembleName")).Clear();
            driver.FindElement(By.Id("EnsembleName")).SendKeys("UNO Jazz");
            driver.FindElement(By.Id("Year")).Click();
            driver.FindElement(By.Id("Year")).Clear();
            driver.FindElement(By.Id("Year")).SendKeys("2016");
            driver.FindElement(By.Name("button")).Click();
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