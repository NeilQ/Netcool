using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Netcool.Api.Domain.Configuration
{
    public static class AppConfigurationExtensions
    {
        public static IConfigurationBuilder AddEfConfiguration(
            this IConfigurationBuilder builder,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            return builder.Add(new EfConfigurationSource(optionsAction));
        }
    }
}