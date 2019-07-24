using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MPW.Data;
using MPW.Services;


namespace XUnitTest_Esolvit.Utilities
{
    public static class Utilities
    {
        public static DbContextOptions<ApplicationDbContext> TestDbContextOptions()
        {
            var OptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            OptionsBuilder.UseMySql("Server=40.113.220.113;Database=testdb5;User=user;Password=Password123!@#;");
            return OptionsBuilder.Options;
        }

        public static DbContextOptions<ApplicationDbContext> TestDbContextOptionsInMemory()
        {
            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and 
            // IServiceProvider that the context should resolve all of its 
            // services from.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }

        public static EmailSenderConfiguation EmailSenderConfigurationBuilder()
        {
            var host = "smtp.office365.com";
            var port = 587;
            var enableSSL = true;
            var username = "esolvittest@outlook.com";
            var password = "Password123!@#";

            var config = new EmailSenderConfiguation(host, port, enableSSL, username, password);

            return config;
        }
    }
}
