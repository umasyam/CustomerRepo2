using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace CustomerRepository.Models
{
    /// <summary>
    /// The admin database context factory.
    /// </summary>
    public class AdminDbContextFactory : IDesignTimeDbContextFactory<AdminDbContext>
    {
        /// <summary>
        /// Creates database context.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The database context.</returns>
        public AdminDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
              .AddJsonFile("appsettings.json")
              .AddEnvironmentVariables()
              .Build();

            //Setting up environment variables.
            var optionsBuilder = new DbContextOptionsBuilder<AdminDbContext>();
            string dncDatasourceUsername = config["dnc.datasource.adminusername"];
            string dncDatasourcepassword = config["dnc.datasource.adminpassword"];
            string dncDatasourceServer = config["dnc.datasource.server"];
            string dncDatasourceDatabase = config["dnc.datasource.database"];
            string dncDatasourcePort = config["dnc.datasource.port"];

            //Setting up connection string.
            string postGresConnectionString =
            $"User ID={dncDatasourceUsername};Server={dncDatasourceServer};Password={dncDatasourcepassword};Port={dncDatasourcePort};Database={dncDatasourceDatabase};Integrated Security=true;Pooling=true;";
            optionsBuilder.UseNpgsql(postGresConnectionString);
            return new AdminDbContext(optionsBuilder.Options);
        }
    }
}