﻿using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Lizst
{
    //Tests to make sure the system does not allow us to add an ensemble
    [TestFixture]
    public class AddNullEnsembleTest
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
        public void TheAddNullEnsembleTest()
        {
            //Details may be different for a new test case
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com/Ensemble");
            driver.FindElement(By.LinkText("Add Ensemble")).Click();
            driver.FindElement(By.Id("Year")).Click();
            driver.FindElement(By.Id("Year")).Clear();
            driver.FindElement(By.Id("Year")).SendKeys("2016");
            driver.FindElement(By.Id("Conductor")).Click();
            driver.FindElement(By.Id("Conductor")).Clear();
            driver.FindElement(By.Id("Conductor")).SendKeys("Matthew Brooks");
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

