using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    public class MessagingTest
    {
        public string urlsplit(string url)
        {
            Console.WriteLine(url);

            //van #forward a végén, nem minden esetben ezért kell elágazás
            if (url.Contains("#"))
            {
                return url.Split('#')[0].Substring(url.Split('#')[0].Length - 36);
            }

            else
            {
                return url.Substring(url.Length - 36);
            }
        }

        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL, testname, url, newurl;
        private bool acceptNextAlert = true;
        private Random r = new Random();
        private int rnd = 0, Size = 25;

        private string RandomString(int Size)
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = input[r.Next(0, input.Length)];
                builder.Append(ch);
            }
            return builder.ToString();
        }

        [SetUp]
        public void SetupTest()
        {
            //driver = new ChromeDriver();
            //driver = new FirefoxDriver();
            driver = new InternetExplorerDriver();
            baseURL = "http://clic-ftest-app1:81/";
            verificationErrors = new StringBuilder();
            rnd = r.Next(1, 100);
            testname = RandomString(Size);
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
        public void TheMessagingTest()
        {
            driver.Navigate().GoToUrl(baseURL); Thread.Sleep(1000);
            driver.Navigate().GoToUrl("http://clic-ftest-app1:81/MessageTemplateList/List?fromMenu=True#forward"); Thread.Sleep(2000);
            driver.FindElement(By.Id("actionButton_New")).Click(); Thread.Sleep(1000);
            try
            {
                driver.FindElement(By.Id("Input_Code")).Clear();
            }
            catch
            {
                driver.FindElement(By.Id("actionButton_New")).Click(); Thread.Sleep(1000);
                driver.FindElement(By.Id("Input_Code")).Clear();

            }
            driver.FindElement(By.Id("Input_Code")).SendKeys(testname);
            new SelectElement(driver.FindElement(By.Id("Input_AddressId"))).SelectByText("ap-dominik.kocka@egroup.hu"); Thread.Sleep(1000);
            new SelectElement(driver.FindElement(By.Id("Input_MessageTopicId"))).SelectByText("automatictest"); Thread.Sleep(1000);
            new SelectElement(driver.FindElement(By.Id("SelectedFieldUid"))).SelectByText("Text display"); Thread.Sleep(1000);
            driver.FindElement(By.Id("newFormField")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.XPath("//td/input")).Clear();
            driver.FindElement(By.XPath("//td/input")).SendKeys("automatictest");
            driver.FindElement(By.Id("actionButton_Save")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("gvMessageTemplate_DXFREditorcol1_I")).SendKeys(testname);Thread.Sleep(2000);


            driver.FindElement(By.Id("gvMessageTemplate_DXDataRow0")).Click();
            try
            {
                url = driver.Url.ToString();
                newurl = "https://clicpltest.egroup.hu/Message/NewMessage?templateId=" + urlsplit(url);
                driver.Navigate().GoToUrl("https://clicpltest.egroup.hu/Login/LoginWithRSADemo"); Thread.Sleep(1000);
            }
            catch
            {
                url = driver.Url.ToString();
                newurl = "https://clicpltest.egroup.hu/Message/NewMessage?templateId=" + urlsplit(url);
                driver.Navigate().GoToUrl("https://clicpltest.egroup.hu/Login/LoginWithRSADemo"); Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("loginId")).Clear();
            driver.FindElement(By.Id("loginId")).SendKeys("100003"); Thread.Sleep(1000);
            driver.FindElement(By.Id("login")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("submit")).Click(); Thread.Sleep(1000);
            driver.Navigate().GoToUrl(newurl); Thread.Sleep(1000);
            driver.FindElement(By.Id("actionButton_Save")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("actionButton_Authorize")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("actionButton_Authorize")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("actionButton_Send")).Click(); Thread.Sleep(1000);
            driver.FindElement(By.Id("actionButton_Send")).Click(); Thread.Sleep(1000);


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
