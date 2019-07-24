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

namespace XUnitTest_Esolvit.PagesUnitTests.Mentor.ProfileBuilder
{
    public class StrategicDomainsTest
    {
        [Fact]
        public void OnGet()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new ApplicationDbContext(optionsBuilder.Options);

            var pageModel = new StrategicDomainModel(mockDb);
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
        public async Task OnPageAsync()
        {

            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();
            var expectedUser = expectedUsers.FirstOrDefault();
            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetMentorAsync(username))
                .Returns(Task.FromResult(expectedUser));

            var strategicDomains = ApplicationDbContext.GetSeedStrategicDomains();

            var pageModel = new StrategicDomainModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.StrategicDomain = strategicDomains.FirstOrDefault();

            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<RedirectToPageResult>(result);
            #endregion
        }

        [Fact]
        public async Task OnPageAsync_InvalidModelState()
        {

            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();
            var expectedUser = expectedUsers.FirstOrDefault();
            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetMentorAsync(username))
                .Returns(Task.FromResult(expectedUser));

            var strategicDomain = ApplicationDbContext.GetSeedStrategicDomains();

            var pageModel = new StrategicDomainModel(mockDb.Object)
            {
                Username = username,
                StrategicDomain = strategicDomain.FirstOrDefault()
            };
            pageModel.ModelState.AddModelError("This is an test error", "Error Message!");

            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<RedirectResult>(result);

            var pageResult = result as RedirectResult;
            var url = pageResult.Url;
            Assert.Contains(
                "Error", url);
            #endregion
        }

        [Fact]
        public async Task OnPostAsync_MentorNull()
        {
            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();
            var expectedUser = expectedUsers.FirstOrDefault();
            expectedUser.Mentor = null;
            var username = expectedUser.UserName;

            var strategicDomains = ApplicationDbContext.GetSeedStrategicDomains();

            mockDb.Setup(db => db.GetMentorAsync(username))
                .Returns(Task.FromResult(expectedUser));

            var pageModel = new StrategicDomainModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.StrategicDomain = strategicDomains.FirstOrDefault();

            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }
    }
}
