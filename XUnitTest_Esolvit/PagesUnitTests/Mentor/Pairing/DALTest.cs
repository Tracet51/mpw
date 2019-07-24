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

namespace XUnitTest_Esolvit.PagesUnitTests.Mentor.Pairing
{
    public class DALTest
    {
        [Fact]
        public async Task AddPair()
        {
            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var username = "test@example.com";
                var users = ApplicationDbContext.GetSeedMentor();

                var seedMentors = ApplicationDbContext.GetSeedMentor();
                await db.AddRangeAsync(seedMentors);
                await db.SaveChangesAsync();

                var expectedUser = seedMentors.Where(u => u.UserName == username).Single();

                var joinCode = Guid.NewGuid().ToString();
                var pair = new Pair
                {
                    DateCreated = DateTime.Now,
                    JoinCode = joinCode,
                    MentorID = expectedUser.Mentor.ID,
                    Mentor = expectedUser.Mentor
                };
                #endregion

                #region Act
                await db.AddPairAsync(pair);

                var expectedPair = await db.Pair.Where(p => p.JoinCode == joinCode).SingleAsync();
                #endregion

                #region Assert
                Assert.True(expectedPair != null);
                Assert.True(expectedPair.MentorID == expectedUser.Mentor.ID);
                #endregion
            }

        }

        [Fact]
        public async Task GetAllPairsForMentor()
        {
            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var username = "test@example.com";

                var seedMentors = ApplicationDbContext.GetSeedMentor();
                await db.AddRangeAsync(seedMentors);
                await db.SaveChangesAsync();

                var expectedUser = seedMentors.Where(u => u.UserName == username).Single();

                var pairs = ApplicationDbContext.GetSeedPairs();
                var pair = pairs.FirstOrDefault();
                pair.MentorID = expectedUser.Mentor.ID;

                await db.AddPairAsync(pair);
                #endregion

                #region Act
                var databasePairs = await db.GetPairsForMentorAsync(expectedUser.Mentor.ID);
                var databasePair = databasePairs.FirstOrDefault();
                #endregion

                #region Assert
                Assert.True(databasePair != null);
                Assert.True(databasePair.MentorID == expectedUser.Mentor.ID);
                Assert.True(databasePair.JoinCode == pair.JoinCode);
                #endregion
            }
        }

        [Fact]
        public async Task GetCoursesForPair()
        {
            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var pair = ApplicationDbContext.GetSeedPairs()[0];
                var mentor = ApplicationDbContext.GetSeedMentor()[0].Mentor;
                var course = ApplicationDbContext.GetSeedCourses()[0];
                
                pair.Mentor = mentor;
                pair.MentorID = mentor.ID;
       
                course.Pair = pair;
                course.Pair.PairID = pair.PairID;

                await db.AddAsync(course);
                await db.SaveChangesAsync();

                #endregion

                #region Act
                var dbCourses = await db.GetCoursesForPairAsync(pair.PairID);
                var dbCourse = dbCourses.FirstOrDefault();
                #endregion

                #region Assert
                Assert.True(dbCourse.PairID == pair.PairID);
                Assert.True(dbCourse.Pair.MentorID == mentor.ID);
                Assert.True(dbCourse.CourseName == course.CourseName);
                #endregion
            }
        }
    }
}
