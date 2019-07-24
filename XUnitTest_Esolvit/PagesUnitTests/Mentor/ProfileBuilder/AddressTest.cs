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
    public class AddressTest
    {
        [Fact]
        public void OnGet()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new ApplicationDbContext(optionsBuilder.Options);

            var pageModel = new AddressModel(mockDb);
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
                .Returns(Task.FromResult(expectedUsers.FirstOrDefault()));

            var pageModel = new AddressModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.AddressMentor = ApplicationDbContext.GetSeedAddresses()
                .FirstOrDefault();

            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<RedirectResult>(result);
            #endregion
        }

        [Fact]
        public async Task OnPageAsync_MentorIsNull()
        {

            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();

            var expectedUser = expectedUsers.FirstOrDefault();
            expectedUser.Mentor = null;

            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetMentorAsync(username))
                .Returns(Task.FromResult(expectedUser));

            var pageModel = new AddressModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.AddressMentor = ApplicationDbContext.GetSeedAddresses()
                .FirstOrDefault();

            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(result);
            #endregion
        }

        [Fact]
        public async Task OnPageAsync_AddressIsNotNull()
        {

            #region Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var expectedUsers = ApplicationDbContext.GetSeedMentor();

            var expectedUser = expectedUsers.FirstOrDefault();
            expectedUser.Mentor.Address = ApplicationDbContext.GetSeedAddresses().FirstOrDefault();

            var username = expectedUser.UserName;

            mockDb.Setup(db => db.GetMentorAsync(username))
                .Returns(Task.FromResult(expectedUser));

            var pageModel = new AddressModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.AddressMentor = ApplicationDbContext.GetSeedAddresses()
                .FirstOrDefault();

            #endregion

            #region Act
            var result = await pageModel.OnPostAsync();
            #endregion

            #region Assert
            Assert.IsType<RedirectResult>(result);

            var redirectResult = result as RedirectResult;
            Assert.EndsWith("Error", redirectResult.Url);
            #endregion
        }
    }
}
