using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
namespace CustomerRepository.Models
{
    /// <summary>
    /// The base database context.
    /// </summary>
    public class BaseDbContext<T> : DbContext where T : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDbContext"/> class.
        /// </summary>
        /// <param name="options">Database context.</param>
        protected BaseDbContext(DbContextOptions<T> options) : base(options)
        {
            SubscribeChangeTrackerEvents();
        }

        /// <summary>
        /// Gets or sets Customer.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// OnModel Creating method.
        /// </summary>
        /// <param name="modelBuilder">Model Builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// On Configuring method.
        /// </summary>
        /// <param name="optionsBuilder">Options Builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            NpgsqlConnection.GlobalTypeMapper.UseJsonNet(settings: new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            //optionsBuilder.UseSnakeCaseNamingConvention();
        }

        /// <summary>
        /// Subscribe Change Tracker Events.
        /// </summary>
        private void SubscribeChangeTrackerEvents()
        {
            ChangeTracker.Tracked += (sender, args) =>
            {
                if (!args.FromQuery)
                {
                    UpdateAuditInfo(args.Entry);
                }
            };

            ChangeTracker.StateChanged += (sender, args) => UpdateAuditInfo(args.Entry);
        }

        /// <summary>
        /// Update Audit Info.
        /// </summary>
        /// <param name="entity">Entity.</param>
        private static void UpdateAuditInfo(EntityEntry entity)
        {
            var now = DateTime.UtcNow;

            switch (entity.State)
            {
                case EntityState.Added:
                    entity.CurrentValues[nameof(AuditableEntity.CreatedDate)] = now;
                    entity.CurrentValues[nameof(AuditableEntity.UpdatedDate)] = now;
                    break;
                case EntityState.Modified:
                    entity.CurrentValues[nameof(AuditableEntity.UpdatedDate)] = now;
                    break;
                default:
                    // No action required for other `EntityState` values.
                    break;
            }
        }
    }
}
