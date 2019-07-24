using MPW.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest_Esolvit.PagesUnitTests.Mentor.Course
{
    public class MentorCourseDALTest
    {
        [Fact]
        public async Task GetCourseInfo()
        {
            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var testMentor = ApplicationDbContext.GetSeedMentor().FirstOrDefault().Mentor;
                var testCourse = ApplicationDbContext.GetSeedCourses().FirstOrDefault();
                var testSession = ApplicationDbContext.GetSeedSessions().FirstOrDefault();
                var testEvent = ApplicationDbContext.GetSeedEvents().FirstOrDefault();
                var testObjective = ApplicationDbContext.GetSeedObjectives().FirstOrDefault();
                var testAssignments = ApplicationDbContext.GetSeedAssignments().FirstOrDefault();
                var testPair = ApplicationDbContext.GetSeedPairs().FirstOrDefault();
                var testDocument = ApplicationDbContext.GetSeedDocuments().FirstOrDefault();
                var testResource = ApplicationDbContext.GetSeedResource().FirstOrDefault();

                testCourse.Objectives = new List<Trello> { testObjective };
                testCourse.Sessions = new List<Session> { testSession };
                testCourse.Events = new List<Event> { testEvent };
                testCourse.Resources = new List<Resource> { testResource };

                testCourse.Pair = testPair;
                testCourse.PairID = testPair.PairID;

                await db.Course.AddAsync(testCourse);
                await db.SaveChangesAsync();
                #endregion

                #region Act
                var dbCourse = await db.GetCourseAsync(testCourse.CourseID);
                #endregion

                #region Assert
                Assert.True(dbCourse != null);
                Assert.True(dbCourse.CourseID == testCourse.CourseID);
                Assert.True(dbCourse.PairID == testCourse.PairID);
                Assert.True(dbCourse.Pair.JoinCode == testCourse.Pair.JoinCode);
                #endregion

                await Task.FromResult(0);
            }

        }
    }
}
