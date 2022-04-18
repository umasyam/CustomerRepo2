using Microsoft.EntityFrameworkCore;
namespace CustomerRepository.Models
{
    /// <summary>
    /// The Admin database context.
    /// </summary>
    public class AdminDbContext : BaseDbContext<AdminDbContext>
    {
        /// <summary>
        /// Model Creation.
        /// </summary>
        /// <param name="modelBuilder">model Builder as input.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
            });

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminDbContext" /> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options)
        {
        }
    }
}