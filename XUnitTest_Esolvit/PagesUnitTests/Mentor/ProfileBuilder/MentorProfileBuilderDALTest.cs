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
    public class MentorProfileBuilderDALTest
    {
        [Fact]
        public async Task GetCurrentUser()
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

                #endregion

                #region Act
                var actualUser = await db.GetAppUserAsync(username);
                #endregion

                #region Assert
                Assert.Equal(
                    expectedUser.UserName,
                    actualUser.UserName);
                #endregion
            }
        }

        [Fact]
        public async Task AddAboutToMentor()
        {

            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var username = "test@example.com";

                var seedMentors = ApplicationDbContext.GetSeedMentor();
                var seedAbouts = ApplicationDbContext.GetSeedAbout();
                
                await db.AddRangeAsync(seedMentors);
                await db.SaveChangesAsync();

                var testUser = seedMentors.Where(u => u.UserName == username).Single();

                #endregion

                #region Act
                var expectedUser = await db
                    .AddMentorAboutAsync(testUser.Mentor, seedAbouts.FirstOrDefault());

                var actualUser = await db.GetAppUserAsync(username);
                #endregion

                #region Assert
                Assert.Equal(
                    expectedUser.About,
                    actualUser.Mentor.About);

                Assert.Equal(
                    seedAbouts.FirstOrDefault(),
                    actualUser.Mentor.About);
                #endregion
            }
        }


        [Fact]
        public async Task AddAddressToMentor()
        {

            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var username = "test@example.com";

                var seedMentors = ApplicationDbContext.GetSeedMentor();
                var seedAddresses = ApplicationDbContext.GetSeedAddresses();

                await db.AddRangeAsync(seedMentors);
                await db.SaveChangesAsync();

                var testUser = seedMentors.Where(u => u.UserName == username).Single();

                #endregion

                #region Act
                var expectedUser = await db
                    .UpdateMentor(testUser.Mentor, seedAddresses.FirstOrDefault());


                var actualUser = await db.GetAppUserAsync(username);
                #endregion

                #region Assert
                Assert.Equal(
                    expectedUser.Address.ID,
                    actualUser.Mentor.Address.ID);

                Assert.Equal(
                    seedAddresses.FirstOrDefault().ID,
                    actualUser.Mentor.Address.ID);
                #endregion
            }
        }

        [Fact]
        public async Task AddCertificateToMentor()
        {

            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var username = "test@example.com";

                var seedMentors = ApplicationDbContext.GetSeedMentor();
                var seedCertificates = ApplicationDbContext.GetSeedCertificates();

                await db.AddRangeAsync(seedMentors);
                await db.SaveChangesAsync();

                var testUser = seedMentors.Where(u => u.UserName == username).FirstOrDefault();

                #endregion

                #region Act
                var expectedUser = await db
                    .UpdateMentor(testUser.Mentor, seedCertificates.FirstOrDefault());


                var actualUser = await db.GetMentorAsync(username);
                #endregion

                #region Assert
                Assert.Equal(
                    expectedUser.Certificates.FirstOrDefault().ID,
                    actualUser.Mentor.Certificates.FirstOrDefault().ID);

                Assert.Equal(
                    seedCertificates.FirstOrDefault().ID,
                    actualUser.Mentor.Certificates.FirstOrDefault().ID);
                #endregion
            }
        }

        [Fact]
        public async Task AddCertificatesToMentor()
        {

            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var username = "test@example.com";

                var seedMentors = ApplicationDbContext.GetSeedMentor();
                var seedCertificates = ApplicationDbContext.GetSeedCertificates();

                await db.AddRangeAsync(seedMentors);
                await db.SaveChangesAsync();

                var testUser = seedMentors.Where(u => u.UserName == username).FirstOrDefault();

                #endregion

                #region Act
                MPW.Data.Mentor expectedUser = new MPW.Data.Mentor(); ;
                foreach (var cert in seedCertificates)
                {
                    expectedUser = await db
                    .UpdateMentor(testUser.Mentor, cert);
                }

                var actualUser = await db.GetMentorAsync(username);
                #endregion

                #region Assert
                foreach (var cert in actualUser.Mentor.Certificates)
                {
                    Assert.Contains(
                        seedCertificates, c => c.ID == cert.ID);

                    Assert.Contains(
                        expectedUser.Certificates, c => c.ID == cert.ID);
                }

                foreach (var cert in seedCertificates)
                {
                    Assert.Contains(
                        actualUser.Mentor.Certificates, c => c.ID == cert.ID);

                    Assert.Contains(
                        expectedUser.Certificates, c => c.ID == cert.ID);
                }
                #endregion
            }
        }

            [Fact]
        public async Task AddStrategicDomainsToMentor()
        {

            using (var db = new ApplicationDbContext(Utilities.Utilities.TestDbContextOptionsInMemory()))
            {
                #region Arrange
                var username = "test@example.com";

                var seedMentors = ApplicationDbContext.GetSeedMentor();
                var seedSDs = ApplicationDbContext.GetSeedStrategicDomains();

                await db.AddRangeAsync(seedMentors);
                await db.SaveChangesAsync();

                var testUser = seedMentors.Where(u => u.UserName == username).FirstOrDefault();

                #endregion

                #region Act
                MPW.Data.Mentor expectedUser = new MPW.Data.Mentor(); ;
                foreach (var sd in seedSDs)
                {
                    expectedUser = await db
                    .UpdateMentor(testUser.Mentor, sd);
                }

                var actualUser = await db.GetMentorAsync(username);
                #endregion

                #region Assert
                foreach (var sd in actualUser.Mentor.StrategicDomains)
                {
                    Assert.Contains(
                        seedSDs, s => s.ID == sd.ID);

                    Assert.Contains(
                        expectedUser.StrategicDomains, s => s.ID == sd.ID);
                }

                foreach (var sd in seedSDs)
                {
                    Assert.Contains(
                        actualUser.Mentor.StrategicDomains, s => s.ID == sd.ID);

                    Assert.Contains(
                        expectedUser.StrategicDomains, s => s.ID == sd.ID);
                }
                #endregion
            }
        }
    }
}
