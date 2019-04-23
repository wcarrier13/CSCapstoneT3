using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    //Tests that a musician is added properly
    [TestFixture]
    public class AddMusicanTest
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
        public void TheAddMusicanTest()
        {
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/Musician");
            driver.FindElement(By.LinkText("Add Musician")).Click();
            driver.FindElement(By.Id("MusicianName")).Click();
            driver.FindElement(By.Id("MusicianName")).Clear();
            driver.FindElement(By.Id("MusicianName")).SendKeys("Maria Stathopoulos");
            driver.FindElement(By.Id("Email")).Click();
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("mstathopoulos@unomaha.edu");
            // ERROR: Caught exception [ERROR: Unsupported command [addSelection | id=Part | label=Violin 1]]
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Instrument'])[1]/following::option[1]")).Click();
            // ERROR: Caught exception [ERROR: Unsupported command [addSelection | id=Part | label=Violin 2]]
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Instrument'])[1]/following::option[2]")).Click();
            driver.FindElement(By.Name("add")).Click();
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
