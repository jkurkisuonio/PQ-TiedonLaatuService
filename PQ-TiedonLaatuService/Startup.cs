using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PQ_TiedonLaatuService.Data;
using PQ_TiedonLaatuService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PQ_TiedonLaatuService
{
    class Startup
    {
    
    public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var appConfig = config.GetSection("application").Get<Application>();

            services.AddDbContext<PrimusAlertContext>(options =>
          options.UseSqlServer(config.GetConnectionString("PrimusAlertContext")));
        }
    
    }
}
