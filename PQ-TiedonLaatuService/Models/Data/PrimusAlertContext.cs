using PQ_TiedonLaatuService.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PQ_TiedonLaatuService.Models.Database;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PQ_TiedonLaatuService.Data
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-3.1&tabs=visual-studio
    /// https://www.entityframeworktutorial.net/efcore/entity-framework-core-console-application.aspx
    /// https://www.entityframeworktutorial.net/efcore/entity-framework-core-console-application.aspx
    /// https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/creating-an-entity-framework-data-model-for-an-asp-net-mvc-application
    /// https://devblogs.microsoft.com/dotnet/announcing-entity-framework-core-3-1-and-entity-framework-6-4/
    /// https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx
    /// 
    /// </summary>
    public class PrimusAlertContext : DbContext
    {
        public virtual DbSet<AlertType> AlertTypes { get; set; }
        public virtual DbSet<PrimusAlert> PrimusAlerts { get; set; }
        public virtual DbSet<AlertReceiver> AlertReceivers { get; set; }

        //public PrimusAlertContext(DbContextOptions<PrimusAlertContext> options) : base(options)
        //{

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlertType>().ToTable("AlertType");
            modelBuilder.Entity<PrimusAlert>().ToTable("PrimusAlert");
            modelBuilder.Entity<AlertReceiver>().ToTable("AlertReceiver");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // Configuration
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var appConfig = config.GetSection("application").Get<Application>();

            //   optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=PrimusAlertDb;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(config.GetConnectionString("PrimusAlertContext"));
        }

    }
}
