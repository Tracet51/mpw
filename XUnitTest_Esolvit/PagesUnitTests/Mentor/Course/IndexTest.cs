using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using MPW.Data;
using XUnitTest_Esolvit.Utilities;
using MPW.Pages.Mentor.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;


namespace XUnitTest_Esolvit.PagesUnitTests.Mentor.Course
{
    public class IndexTest
    {

        public Tuple<MPW.Data.Course, MPW.Data.AppUser> GetCommonSetup()
        {
            var mentor = ApplicationDbContext.GetSeedMentor().FirstOrDefault();
            var course = ApplicationDbContext.GetSeedCourses().FirstOrDefault();
            var events = ApplicationDbContext.GetSeedEvents();
            var sessions = ApplicationDbContext.GetSeedSessions();
            var resources = ApplicationDbContext.GetSeedResource();
            var objectives = ApplicationDbContext.GetSeedObjectives();
            var pair = ApplicationDbContext.GetSeedPairs().FirstOrDefault();

            course.Events = events;
            course.Sessions = sessions;
            course.Resources = resources;
            course.Objectives = objectives;
            course.Pair = pair;
            course.Pair.MentorID = mentor.Mentor.ID;

            return new Tuple<MPW.Data.Course, MPW.Data.AppUser>(course, mentor);
        }
        
        /*[Fact]
        public async Task OnGetAsyncTest()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var (course, mentor) = GetCommonSetup();

            var pageModel = new IndexModel(mockDb.Object);
            pageModel.Username = mentor.UserName;


            mockDb.Setup(db => db.GetMentorAsync(mentor.UserName))
                .Returns(Task.FromResult(mentor));

            mockDb.Setup(db => db.GetCourseAsync(course.CourseID))
                .Returns(Task.FromResult(course));

            #endregion

            #region Act
            var page = await pageModel.OnGetAsync(course.CourseID);
            #endregion

            #region Assert
            Assert.IsType<PageResult>(page);
            #endregion
        }*/

        /*
        [Fact]
        public async Task OnGetAsync_NullMentorTest()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var (course, mentor) = GetCommonSetup();

            // Failure Point
            mentor.Mentor = null;

            var pageModel = new IndexModel(mockDb.Object);
            pageModel.Username = mentor.UserName;

            mockDb.Setup(db => db.GetMentorAsync(mentor.UserName))
                .Returns(Task.FromResult(mentor));

            mockDb.Setup(db => db.GetCourseAsync(course.CourseID))
                .Returns(Task.FromResult(course));
            #endregion

            #region Act
            var page = await pageModel.OnGetAsync(course.CourseID);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(page);
            #endregion
        }
        */

        /*
        [Fact]
        public async Task OnGetAsync_MentorIDNotMatchTest()
        {
            #region Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            var mockDb = new Mock<ApplicationDbContext>(optionsBuilder.Options);

            var (course, mentor) = GetCommonSetup();

            // Failure Point
            mentor.Mentor.ID = -1;

            var pageModel = new IndexModel(mockDb.Object);
            pageModel.Username = mentor.UserName;

            mockDb.Setup(db => db.GetMentorAsync(mentor.UserName))
                .Returns(Task.FromResult(mentor));

            mockDb.Setup(db => db.GetCourseAsync(course.CourseID))
                .Returns(Task.FromResult(course));
            #endregion

            #region Act
            var page = await pageModel.OnGetAsync(course.CourseID);
            #endregion

            #region Assert
            Assert.IsType<NotFoundObjectResult>(page);
            #endregion
        }
        */
    }
}
