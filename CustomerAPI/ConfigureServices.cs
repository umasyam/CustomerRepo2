using CustomerRepository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using CustomerRepository.Models;
namespace CustomerAPI
{
    /// <summary>
    /// Configure Services class.
    /// </summary>
    public static class ConfigureServices
    {
        /// <summary>
        /// Add Infrastructure
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <returns>Service Response.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserDbContext>(provider => provider.GetService<UserDbContext>());

            return services;
        }
    }
}