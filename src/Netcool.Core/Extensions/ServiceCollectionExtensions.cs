using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Netcool.Core.Repositories;
using Netcool.Core.Services;

namespace Netcool.Core;

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

    public static List<TypeInfo> GetDomainRepositoryTypes(this Assembly assembly)
    {
        var typeInfoList = assembly.DefinedTypes
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.Name.Contains("Repository")
                        && x.GetInterfaces().Any(i => i == typeof(IRepository)))
            .ToList();
        /*
           var typeInfoList = assembly.DefinedTypes
            .Where(x => x.IsClass
                        && !x.IsAbstract
                        && x.Name.EndsWith("Repository")
                        && x.GetInterfaces().Any(i => i == typeof(IRepository<>) || i == typeof(IRepository<,>)))
            .ToList();
         */

        return typeInfoList;
    }

    public static void AddDomainRepositoryTypes(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var types = assembly.GetDomainRepositoryTypes();
        foreach (var type in types)
        {
            if (type.IsGenericTypeDefinition) continue;
            foreach (var implementedInterface in type.ImplementedInterfaces)
            {
                if (implementedInterface.GenericTypeArguments.Length != type.GenericTypeParameters.Length) continue;
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
        }
    }

    public static void AddDomainServiceTypes(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var types = assembly.GetDomainServiceTypes();
        foreach (var type in types)
        {
            if (type.IsGenericTypeDefinition) continue;
            foreach (var implementedInterface in type.ImplementedInterfaces)
            {
                if (implementedInterface.GenericTypeArguments.Length != type.GenericTypeParameters.Length) continue;
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
        }
    }
}
