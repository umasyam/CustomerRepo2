using System.Threading;
using System.Threading.Tasks;
using CustomerRepository.Models;
using Microsoft.EntityFrameworkCore;
namespace CustomerRepository.Interfaces
{
    /// <summary>
    /// UserDbContext Interface.
    /// </summary>
    public interface IUserDbContext
    {
        /// <summary>
        /// Policy References.
        /// </summary>
        DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Save Changes To DB.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
