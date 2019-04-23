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
    [TestFixture]
    public class EditScoreTest
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private bool acceptNextAlert = true;

        [SetUp]
        public void SetupTest()
        {
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
        public void TheEditScoreTest()
        {
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/Score");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Classical'])[3]/following::a[1]")).Click();
            driver.FindElement(By.Id("Score_Composer")).Click();
            driver.FindElement(By.Id("Score_Composer")).Clear();
            driver.FindElement(By.Id("Score_Composer")).SendKeys("John Williams");
            driver.FindElement(By.Id("Score_Publisher")).Click();
            driver.FindElement(By.Id("Score_Publisher")).Clear();
            driver.FindElement(By.Id("Score_Publisher")).SendKeys("Penguin Books");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Students also have Omaha Symphony parts'])[1]/following::input[2]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Edit Pieces'])[1]/following::button[1]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.Id("numberPicker")).Click();
            driver.FindElement(By.Id("numberPicker")).Clear();
            driver.FindElement(By.Id("numberPicker")).SendKeys("2");
            driver.FindElement(By.Name("results[0][2]")).Click();
            new SelectElement(driver.FindElement(By.Name("results[0][2]"))).SelectByText("Good");
            driver.FindElement(By.Name("results[0][2]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Rating:'])[62]/following::input[2]")).Click();
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
