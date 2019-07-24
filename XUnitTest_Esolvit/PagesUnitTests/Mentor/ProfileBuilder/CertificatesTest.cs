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
    public class CertificatesTest
    {
        [Fact]
        public void OnGet()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new ApplicationDbContext(optionsBuilder.Options);

            var pageModel = new CertificatesModel(mockDb);
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

            var certificates = ApplicationDbContext.GetSeedCertificates();

            var pageModel = new CertificatesModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.Certificate = certificates.FirstOrDefault();

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

            var certificates = ApplicationDbContext.GetSeedCertificates();

            var pageModel = new CertificatesModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.Certificate = certificates.FirstOrDefault();
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

            var certificates = ApplicationDbContext.GetSeedCertificates();

            mockDb.Setup(db => db.GetMentorAsync(username))
                .Returns(Task.FromResult(expectedUser));

            var pageModel = new CertificatesModel(mockDb.Object);
            pageModel.Username = username;
            pageModel.Certificate = certificates.FirstOrDefault();

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
