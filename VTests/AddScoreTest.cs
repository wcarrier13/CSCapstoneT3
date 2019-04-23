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
    //Tests that the system can add a score properly
    [TestFixture]
    public class AddScoreTest
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
        public void TheAddScoreTest()
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/Score");
            driver.FindElement(By.LinkText("Add Score")).Click();
            driver.FindElement(By.Id("Title")).Click();
            driver.FindElement(By.Id("Title")).Clear();
            driver.FindElement(By.Id("Title")).SendKeys("The New World Symphony");
            driver.FindElement(By.Id("Composer")).Click();
            driver.FindElement(By.Id("Composer")).Clear();
            driver.FindElement(By.Id("Composer")).SendKeys("Dvorak");
            driver.FindElement(By.Id("Genre")).Click();
            new SelectElement(driver.FindElement(By.Id("Genre"))).SelectByText("Classical");
            driver.FindElement(By.Id("Genre")).Click();
            driver.FindElement(By.Id("Notes")).Click();
            driver.FindElement(By.XPath("//body")).Click();
            driver.FindElement(By.Id("Notes")).Clear();
            driver.FindElement(By.Id("Notes")).SendKeys("Students also have Omaha Symphony parts");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Notes:'])[1]/following::input[1]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Add Pieces'])[1]/following::button[1]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.Id("numberPicker")).Click();
            driver.FindElement(By.Id("numberPicker")).Clear();
            driver.FindElement(By.Id("numberPicker")).SendKeys("3");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Flute'])[1]/following::div[3]")).Click();
            driver.FindElement(By.Id("textbox")).Clear();
            driver.FindElement(By.Id("textbox")).SendKeys("First");
            driver.FindElement(By.Id("results[0][2]")).Click();
            new SelectElement(driver.FindElement(By.Id("results[0][2]"))).SelectByText("Fair");
            driver.FindElement(By.Id("results[0][2]")).Click();
            driver.FindElement(By.Name("results[2][0]")).Click();
            driver.FindElement(By.Name("results[2][0]")).Clear();
            driver.FindElement(By.Name("results[2][0]")).SendKeys("4");
            driver.FindElement(By.Name("results[2][1]")).Click();
            driver.FindElement(By.Name("results[2][1]")).Click();
            // ERROR: Caught exception [ERROR: Unsupported command [doubleClick | name=results[2][1] | ]]
            driver.FindElement(By.Name("results[2][1]")).Clear();
            driver.FindElement(By.Name("results[2][1]")).SendKeys("Second");
            driver.FindElement(By.Id("results[2][2]")).Click();
            new SelectElement(driver.FindElement(By.Id("results[2][2]"))).SelectByText("Aweful");
            driver.FindElement(By.Id("results[2][2]")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Rating:'])[9]/following::button[1]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.Name("results[11][0]")).Click();
            driver.FindElement(By.Name("results[11][0]")).Clear();
            driver.FindElement(By.Name("results[11][0]")).SendKeys("1");
            driver.FindElement(By.Name("results[11][1]")).Click();
            driver.FindElement(By.Id("results[11][2]")).Click();
            new SelectElement(driver.FindElement(By.Id("results[11][2]"))).SelectByText("Excellent");
            driver.FindElement(By.Id("results[11][2]")).Click();
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
