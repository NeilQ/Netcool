using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Netcool.Core.Services;

namespace Netcool.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static List<TypeInfo> GetDomainServiceTypes(this Assembly assembly)
        {
            var typeInfoList = assembly.DefinedTypes
                .Where(x => x.IsClass
                            && !x.IsAbstract
                            && x.Name.EndsWith("Service")
                            && x.GetInterfaces().Any(i => i == typeof(IService)))
                .ToList();

            return typeInfoList;
        }

        public static void AddDomainServiceTypes(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            assembly.GetDomainServiceTypes().ForEach((type) =>
            {
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    switch (lifetime)
                    {
                        case ServiceLifetime.Scoped:
                            services.AddScoped(implementedInterface, type);
                            break;
                        case ServiceLifetime.Singleton:
                            services.AddSingleton(implementedInterface, type);
                            break;
                        case ServiceLifetime.Transient:
                            services.AddTransient(implementedInterface, type);
                            break;
                        default:
                            services.AddTransient(implementedInterface, type);
                            break;
                    }
                }
            });
        }
    }
}