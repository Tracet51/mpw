using System;
using Moq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using MPW.Data;
using MPW.Pages.Mentor.Pairing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MPW.Services;

namespace XUnitTest_Esolvit.PagesUnitTests.Mentor.Pairing
{
    public class CreateTest
    {
        /*
        [Fact]
        public void OnGet()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new ApplicationDbContext(optionsBuilder.Options);

            var emailConfig = Utilities.Utilities.EmailSenderConfigurationBuilder();
            var mockEmail = new Mock<EmailSender>(emailConfig);
            
            var pageModel = new CreateModel(mockDb, mockEmail.Object);

            #endregion

            #region Act
            var page = pageModel.OnGet();
            #endregion

            #region Assert
            Assert.Equal(
                typeof(PageResult),
                page.GetType());
            #endregion
        }
        */

        /*
        [Fact]
        public async Task OnPostAsync()
        {
            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var emailConfig = Utilities.Utilities.EmailSenderConfigurationBuilder();
            var mockEmail = new EmailSender(
                emailConfig.Host,
                emailConfig.Port,
                emailConfig.EnableSSL,
                emailConfig.Username,
                emailConfig.Password);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();
            var expectedUser = expectedUsers.FirstOrDefault();
            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetMentorAsync(username))
                .Returns(Task.FromResult(expectedUsers.FirstOrDefault()));

            var pageModel = new CreateModel(mockDb.Object, mockEmail);
            pageModel.Username = username;
            pageModel.Input = new CreateModel.InputModel
            {
                ClientEmailAddress = "tracet51@live.com",
                EstimatedCompletionDate = new DateTime(2000,01,01),
                ProtegeEmailAddress = "tracet51@live.com"
            };
            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<RedirectToPageResult>(result);
            #endregion
        }
        */

        /*
        [Fact]
        public async Task OnPostAsync_ModelStateInvalid()
        {
            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var emailConfig = Utilities.Utilities.EmailSenderConfigurationBuilder();
            var mockEmail = new Mock<EmailSender>(emailConfig);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();
            var expectedUser = expectedUsers.FirstOrDefault();
            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetAppUserAsync(username))
                .Returns(Task.FromResult(expectedUsers.FirstOrDefault()));


            var message = "Welcome to the Mentor Protege Program. Please use the Code Below to join!:";
            var subject = "Join Mentor-Protege Program";
            mockEmail.Setup(e => e.SendEmailAsync("", subject, message))
                .Returns(Task.FromResult(0));

            var pageModel = new CreateModel(mockDb.Object, mockEmail.Object);
            pageModel.Username = username;
            pageModel.Input = new CreateModel.InputModel
            {
                ProtegeEmailAddress = "",
                ClientEmailAddress = "",
                EstimatedCompletionDate = DateTime.Now
            };

            var modelState = new ModelStateDictionary();
            pageModel.ModelState.AddModelError("Fake Error", "This is a fake error for unit testing");
            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<PageResult>(result);
            #endregion
        }
        */
    }
}
