using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Netcool.Core.WebApi.ValueProviders
{
    public static class SnakeCaseQueryValueProviderExtentions
    {
        public static void ConfigureSnakeCaseQueryString(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(options =>
            {
                options.ValueProviderFactories.Add(new SnakeCaseQueryValueProviderFactory());
            });
            services.TryAddEnumerable(ServiceDescriptor
                .Transient<IApiDescriptionProvider, SnakeCaseQueryParametersApiDescriptionProvider>());
        }
    }
}