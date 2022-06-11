using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;

namespace ContactBook.DesktopTests
{
    public class Tests
    {
        private WindowsDriver<WindowsElement> driver;
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";

        private const string AppPath =
            @"C:\Users\Fu3l\Desktop\ContactBook-DesktopClient\ContactBook-DesktopClient.exe";

        private const string ContactBookApiAddress = "https://contactbook.fu3lxx.repl.co/api";
        private AppiumOptions options;

            [SetUp]
        public void Setup()
        {
            this.options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", AppPath);
            this.driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServer), options);
        }

        [TearDown]
        public void TearDown()
        {
            driver.CloseApp();
            driver.Quit();
        }

        [Test]
        public void Test1()
        {
            var apiTextBox = driver.FindElementByAccessibilityId("textBoxApiUrl");
            apiTextBox.SendKeys(ContactBookApiAddress);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            var windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var nameSearchBox = driver.FindElementByAccessibilityId("textBoxSearch");
            nameSearchBox.SendKeys("steve");

            var searchButton = driver.FindElementByAccessibilityId("buttonSearch");
            searchButton.Click();

            Thread.Sleep(2500);
            
            var firstName = driver.FindElementByXPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]");
            var lastName = driver.FindElementByXPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]");
            
            Assert.That(firstName.Text.Equals("Steve"));
            Assert.That(lastName.Text.Equals("Jobs"));
        }
    }
}