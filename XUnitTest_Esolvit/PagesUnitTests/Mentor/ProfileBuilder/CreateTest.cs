using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using MPW.Data;
using XUnitTest_Esolvit.Utilities;
using MPW.Pages.Mentor.ProfileBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace XUnitTest_Esolvit.PagesUnitTests.Mentor.ProfileBuilder
{
    public class CreateTest
    {
        [Fact]
        public void OnGet()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new ApplicationDbContext(optionsBuilder.Options);

            var pageModel = new CreateModel(mockDb);

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

        [Fact]
        public async Task OnPostAsync()
        {
            #region Arrange
            
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);
            
            var expectedUsers = ApplicationDbContext.GetSeedMentor();
            var expectedUser = expectedUsers.FirstOrDefault();
            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetAppUserAsync(username))
                .Returns(Task.FromResult(expectedUsers.FirstOrDefault()));

            var pageModel = new CreateModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.Input = new CreateModel.InputModel
            {
                About = ApplicationDbContext.GetSeedAbout().FirstOrDefault()
            };
            
            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<RedirectResult>(result);
            #endregion

        }

        [Fact]
        public async Task OnPostAsync_ModelStateInvalid()
        {
            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();
            var expectedUser = expectedUsers.FirstOrDefault();
            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetAppUserAsync(username))
                .Returns(Task.FromResult(expectedUsers.FirstOrDefault()));

            var pageModel = new CreateModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.Input = new CreateModel.InputModel
            {
                About = ApplicationDbContext.GetSeedAbout().FirstOrDefault()
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
    }
}
