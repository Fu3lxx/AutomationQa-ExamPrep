using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ContactBook.SeleniumUiTests
{
    public class Tests
    {
        private WebDriver driver;
        private Random random;


            [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            this.driver.Manage().Window.Maximize();
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            this.random = new Random();
            
        }

        [TearDown]
        public void TearDown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ContactList_CheckFirstClient()
        {
            driver.Navigate().GoToUrl("https://contactbook.fu3lxx.repl.co/contacts");

            var firstClientFirstName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td"));
            var firstClientLasttName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td"));


            Assert.AreEqual("Steve",firstClientFirstName.Text);
        }

        [Test]
        public void Test_SearchWithKeyword_CheckResult()
        {
            driver.Navigate().GoToUrl("https://contactbook.fu3lxx.repl.co/");
            var searchMenu = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));
            searchMenu.Click();

            var keywordBox = driver.FindElement(By.CssSelector("#keyword"));
            keywordBox.SendKeys("albert");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            var firstNameBox = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.fname > td"));
            var lastNameBox = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.lname > td"));
            

            Assert.AreEqual("Albert", firstNameBox.Text);
            Assert.AreEqual("Einstein", lastNameBox.Text);
        }

        [Test]
        public void Test_SearchWithInvalidKeyword_CheckResult()
        {
            driver.Navigate().GoToUrl("https://contactbook.fu3lxx.repl.co/");
            var searchMenu = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));
            searchMenu.Click();

            var keywordBox = driver.FindElement(By.CssSelector("#keyword"));
            keywordBox.SendKeys("invalid2635");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            var resultTextMessage = driver.FindElement(By.CssSelector("#searchResult"));

            Assert.AreEqual("No contacts found.", resultTextMessage.Text);
        }

        [Test]
        public void Test_CreateNewContactWithInvalidData_CheckResult()
        {
            driver.Navigate().GoToUrl("https://contactbook.fu3lxx.repl.co/");
            var createMenu = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(3) > a"));
            createMenu.Click();

            var firstNameBox = driver.FindElement(By.Id("firstName"));
            var lastNameBox = driver.FindElement(By.Id("lastName"));
            var emailBox = driver.FindElement(By.Id("email"));
            var phoneBox = driver.FindElement(By.Id("email"));
            var commentsBox = driver.FindElement(By.Id("comments"));

            firstNameBox.SendKeys("Olivie" + random.Next(0,1000));
            emailBox.SendKeys("SHampen@gmail.com");
            phoneBox.SendKeys("089999999");
            commentsBox.SendKeys("testComment");

            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();

            var resultTextMessage = driver.FindElement(By.CssSelector("body > main > div"));

            Assert.AreEqual("Error: Last name cannot be empty!", resultTextMessage.Text);
        }

        [Test]
        public void Test_CreateNewContactWithValidData_CheckResult()
        {
            driver.Navigate().GoToUrl("https://contactbook.fu3lxx.repl.co/");
            var createMenu = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(3) > a"));
            createMenu.Click();

            var firstNameBox = driver.FindElement(By.Id("firstName"));
            var lastNameBox = driver.FindElement(By.Id("lastName"));
            var emailBox = driver.FindElement(By.Id("email"));
            var phoneBox = driver.FindElement(By.Id("email"));
            var commentsBox = driver.FindElement(By.Id("comments"));

            string randomCode = random.Next(0, 1000).ToString();
            firstNameBox.SendKeys("Olivie" + randomCode);
            lastNameBox.SendKeys("Shampen" + randomCode);
            emailBox.SendKeys("SHampen@gmail.com");
            phoneBox.SendKeys("089999999");
            commentsBox.SendKeys("testComment");

            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();

            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();
            var firstNameOfLastContact = lastContact.FindElement(By.CssSelector("tbody > tr.fname > td"));
            var lastNameOfLastContact = lastContact.FindElement(By.CssSelector("tbody > tr.lname > td"));

            Assert.AreEqual("Olivie" + randomCode, firstNameOfLastContact.Text);
            Assert.AreEqual("Olivie" + randomCode, firstNameOfLastContact.Text);
        }
    }
}