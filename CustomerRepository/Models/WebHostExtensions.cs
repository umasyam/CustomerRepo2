using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;
namespace CustomerRepository.Models
{
    /// <summary>
    /// WebHost Extensions.
    /// </summary>
    public static class WebHostExtensions
    {
        /// <summary>
        /// Migrate DbContext method.
        /// </summary>
        /// <param name="host">WebHost.</param>
        /// <param name="seeder">Seeder.</param>
        /// <returns>Host.</returns>
        public static IWebHost MigrateDbContext<TContext>(this IWebHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                    //InvokeSeeder(host, seeder, context, services);
                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                    throw; // Rethrow under k8s because we rely on k8s to re-run the pod
                }
            }

            // Return
            return host;
        }

        /// <summary>
        /// Invoke Seeder method.
        /// </summary>
        /// <param name="host">WebHost.</param>
        /// <param name="seeder">Seeder.</param>
        /// <param name="context">Context.</param>
        /// <param name="services">Services.</param>
        private static void InvokeSeeder<TContext>(IWebHost host, Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            var logger = services.GetRequiredService<ILogger<TContext>>();
            string dncversion = host.GetMigrationRollbackVerstion();
            var all = context.GetService<IMigrationsAssembly>().Migrations.Select(m => m.Key);
            var applied = context.GetService<IHistoryRepository>().GetAppliedMigrations().Select(m => m.MigrationId);
            var pending = all.Except(applied).ToArray();

            bool isRollBackRequired = host.GetRollbackRequired();
            if (isRollBackRequired)
            {
                if (string.IsNullOrEmpty(dncversion))
                {
                    logger.LogInformation($"Rollback version number is not supplied");
                    throw new ArgumentNullException(nameof(dncversion));
                }
                if (!(all.Contains(dncversion)))
                {

                    throw new Exception($"Rollback version number : {dncversion} not exists.");
                }

                var migrator = context.GetService<IMigrator>();
                logger.LogInformation($"Rollbacking to till version: {dncversion}");
                migrator.Migrate(dncversion);
                seeder(context, services);
                logger.LogInformation($"Rollback completed.");
            }
            else
            {
                for (int i = 0; i < pending.Length; i++)
                {
                    logger.LogInformation($"Applying Pending Migration: {pending[i]} {i + 1}/{pending.Length}");

                }
                context.Database.Migrate();
                seeder(context, services);
            }
        }

        /// <summary>
        /// Get Migration Rollback Verstion method.
        /// </summary>
        /// <param name="host">WebHost.</param>
        /// <returns>version Details.</returns>
        public static string GetMigrationRollbackVerstion(this IWebHost host)
        {
            var cfg = host.Services.GetService<IConfiguration>();
            var versionDetails = cfg.GetValue<string>("dnc.migration.rollbackto.version");
            return versionDetails;
        }

        /// <summary>
        /// Get Rollback Required.
        /// </summary>
        /// <param name="host">WebHost.</param>
        /// <returns>Rollback Required Status.</returns>
        public static bool GetRollbackRequired(this IWebHost host)
        {
            var cfg = host.Services.GetService<IConfiguration>();
            var rollbackrequired = cfg.GetValue<bool>("dnc.migration.rollback.required");
            return rollbackrequired;
        }
    }

}