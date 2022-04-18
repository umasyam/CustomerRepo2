using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
namespace CustomerAPI.MappingProfiles
{
    /// <summary>
    /// Mapping Profile.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        ///  Mapping Profile.
        /// </summary>
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Apply Mappings From Assembly.
        /// </summary>
        /// <param name="assembly">Assembly.</param>
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod("Mapping")
                                 ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }

    /// <summary>
    /// Mapping Interface.
    /// </summary>
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}