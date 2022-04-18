using AutoMapper;
using CustomerDomainModel.Models;
using CustomerRepository.Models;
namespace CustomerAPI.MappingProfiles
{
    /// <summary>
    /// CustomerMappingProfile class.
    /// </summary>
    public class CustomerMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerMappingProfile"/> class.
        /// </summary>
        public CustomerMappingProfile()
        {
            // Map Customer Details
            CreateMap<Customer, CustomerDetail>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}