using Xunit;
using MPW.Data;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using OpenQA.Selenium.Firefox;

namespace XUnitTest_Esolvit
{
    public class MentorUnitTests
    {
        [Fact]
        public void Test1()
        {
            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptions()))
            {
                Mentor testMentor1 = new Mentor();
                //AppUser user = new AppUser();
                //user.CompanyName = "Test Company";
                //testMentor1.AppUser = user;
                testMentor1.About = "Ron's mentor unit test";

                Address testAddress1 = new Address();
                testAddress1.City = "Austin";
                //Need to include address city info
                testAddress1.StreetAddress = "123 Candycane Lane";
                testAddress1.ZipCode = "28473";
                testAddress1.State = "TX";
                db.Address.Add(testAddress1);

                testMentor1.Address = testAddress1;
                db.Mentor.Add(testMentor1);
                db.SaveChanges();

                Mentor mentorRetrieval = db.Mentor.FirstOrDefault(m => m.About == "Ron's mentor unit test");
                Address addressRetrieval = db.Address.FirstOrDefault(a => a.StreetAddress == "123 Candycane Lane");
                Assert.True(mentorRetrieval.Address == addressRetrieval);
                //Assert.True(mentorRetrieval.AppUser.CompanyName == "Test Company");
                Assert.True(mentorRetrieval.About == "Ron's mentor unit test");
                Assert.True(addressRetrieval.City == "Austin");
                Assert.True(addressRetrieval.ZipCode == "28473");
                Assert.True(addressRetrieval.State == "TX");
                db.Remove(db.Mentor.FirstOrDefault(a => a.About == "Ron's mentor unit test"));
                db.Remove(db.Address.FirstOrDefault(a => a.ZipCode == "28473"));
            }
        }

        [Fact]
        public void TestWithFirefoxDriver()
        {

            var email = "test@selenium.org";
            var companyName = "Selenium Inc.";
            var businessField = "Software";
            var phoneNumber = "5125555555";
            var password = "TestAccount1!";
            var userType = "Mentor";
            var aboutCompany = "Random Inc. is the randomest company out there!";
            var streetNumber = "1111";
            var streetAddress = "Random St.";
            var streetAddress2 = "Unit 11";
            var zipCode = "11111";
            var state = "Texas";


            using (var driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(@"https://localhost:44396/Identity/Account/Register");
                driver.FindElement(By.Id("Input_Email")).SendKeys(email);
                driver.FindElement(By.Id("Input_CompanyName")).SendKeys(companyName);
                driver.FindElement(By.Id("Input_Field")).SendKeys(businessField);
                driver.FindElement(By.Id("Input_PhoneNumber")).SendKeys(phoneNumber);
                driver.FindElement(By.Id("Input_AlternatePhoneNumber")).SendKeys(phoneNumber);
                driver.FindElement(By.Id("Input_Password")).SendKeys(password);
                driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys(password);
                driver.FindElement(By.XPath("//*[@class='btn btn-default']/ul/li[3]")).Click();

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_TypeId")));

                SelectElement userTypeHTML = new SelectElement(driver.FindElement(By.Id("Input_TypeId")));
                userTypeHTML.SelectByText(userType);
                driver.FindElement(By.XPath("//*[@class='btn btn-default']/ul/li[3]")).Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_About")));

                driver.FindElement(By.Id("Input_About")).SendKeys(aboutCompany);
                driver.FindElement(By.XPath("//*[@class='btn btn-default']/ul/li[3]")).Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("AddressMentor_StreetNumber")));

                driver.FindElement(By.Id("AddressMentor_StreetNumber")).SendKeys(streetNumber);
                driver.FindElement(By.Id("AddressMentor_StreetAddress")).SendKeys(streetAddress);
                driver.FindElement(By.Id("AddressMentor_StreetAddress2")).SendKeys(streetAddress2);
                driver.FindElement(By.Id("AddressMentor_ZipCode")).SendKeys(zipCode);
                driver.FindElement(By.Id("AddressMentor_State")).SendKeys(state);
                driver.FindElement(By.XPath("//*[@class='btn btn-default']/ul/li[3]")).Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class='btn btn-danger']/ul/li[3]")));

                driver.FindElement(By.XPath("//*[@class='btn btn-danger']/ul/li[3]")).Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@class='btn btn-danger']/ul/li[3]")));

                driver.FindElement(By.XPath("//*[@class='btn btn-danger']/ul/li[3]")).Click();

                Assert.True(driver.Url == "https://localhost:44396/Identity/Account/Manage");
                Assert.True(driver.FindElement(By.Id("Username")).Text.ToString() == email);
                Assert.True(driver.FindElement(By.Id("Input_PhoneNumber")).Text.ToString() == phoneNumber);

                using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptions()))
                {
                    Mentor mentorRetrieval = db.Mentor.FirstOrDefault(m => m.About == aboutCompany);
                    Assert.True(mentorRetrieval != null);
                    Assert.True(mentorRetrieval.AppUser.UserName == email);
                }


                //var jsToBeExecuted = $"window.scroll(0, {link.Location.Y});";
                //((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);
                //var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                //var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("TFS Test API")));
                //clickableElement.Click();
            }
        }
    }
}
