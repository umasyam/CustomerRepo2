using Microsoft.EntityFrameworkCore;
using CustomerRepository.Interfaces;
namespace CustomerRepository.Models
{
    /// <summary>
    /// User database context.
    /// </summary>
    public class UserDbContext : BaseDbContext<UserDbContext>, IUserDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDbContext" /> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}