using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Web;
using System.Text;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace Lizst
{
    public class TestDemo
    {
        IWebDriver driver = new ChromeDriver(@"C:\Users\mchry\Downloads\chromedriver_win32");

        [Test]
        public void titleTest()
        {
            driver.Navigate().GoToUrl("http://ec2-3-16-188-153.us-east-2.compute.amazonaws.com");
            Assert.AreEqual("Home Page - UNO Music Library", driver.Title);
            driver.Close();
            driver.Quit();
        }
    }
}
