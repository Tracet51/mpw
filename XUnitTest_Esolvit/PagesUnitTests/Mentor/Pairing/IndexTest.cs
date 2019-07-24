using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using MPW.Data;
using XUnitTest_Esolvit.Utilities;
using MPW.Pages.Mentor.Pairing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace XUnitTest_Esolvit.PagesUnitTests.Mentor.Pairing
{
    public class IndexTest
    {
        [Fact]
        public async Task OnGetAsync()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var pageModel = new IndexModel(mockDb.Object);

            var mentors = ApplicationDbContext.GetSeedMentor();
            var pairs = ApplicationDbContext.GetSeedPairs();

            for (int i = 0; i < pairs.Count; i++)
            {
                pairs[i].Mentor = mentors[i].Mentor;
            }

            var mentor = mentors[0];
            pageModel.Username = mentor.UserName;

            mockDb.Setup(db => db.GetMentorAsync(mentor.UserName))
                .Returns(Task.FromResult(mentor));

            mockDb.Setup(db => db.GetPairsForMentorAsync(mentors[0].Mentor.ID))
                .Returns(Task.FromResult(pairs.Where(p => p.MentorID == mentor.Mentor.ID).ToList() as IList<Pair>));

            #endregion

            #region Act
            var page = await pageModel.OnGetAsync();
            #endregion

            #region Assert
            Assert.IsType<PageResult>(page);
            #endregion
        }

        [Fact]
        public async Task OnGetAsync_MentorNull()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var pageModel = new IndexModel(mockDb.Object);
            pageModel.Username = "";

            var mentors = ApplicationDbContext.GetSeedMentor();
            var pairs = ApplicationDbContext.GetSeedPairs();

            var mentor = mentors[0];
            var mentorId = mentor.Mentor.ID;
            mentor.Mentor = null;

            mockDb.Setup(db => db.GetMentorAsync(mentor.UserName))
                .Returns(Task.FromResult(mentor));

            mockDb.Setup(db => db.GetPairsForMentorAsync(mentorId))
                .Returns(Task.FromResult(pairs.Where(p => p.MentorID == mentorId).ToList() as IList<Pair>));

            #endregion

            #region Act
            var page = await pageModel.OnGetAsync();
            #endregion

            #region Assert
            Assert.IsType<RedirectResult>(page);

            var result = page as RedirectResult;
            Assert.Contains("/Error", result.Url);
            #endregion
        }
    }
}
