using IBAM.API.Data;
using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(IBAM.API.StartUp))]



namespace IBAM.API
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<IBAM.API.Data.DataContext>(
              options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
        }
    }
}