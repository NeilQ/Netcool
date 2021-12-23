using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Netcool.Swashbuckle.AspNetCore
{
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Include all xml files from AppContext.BaseDirectory
        /// </summary>
        /// <param name="options"></param>
        /// <param name="includeControllerXmlComments">
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </param>
        public static void IncludeAllXmlComments(this SwaggerGenOptions options,
            bool includeControllerXmlComments = false)
        {
            var xmlFiles = Directory
                .EnumerateFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories);
            //.Where(s => Path.GetExtension(s)?.ToLowerInvariant() == ".xml");
            if (!xmlFiles.Any()) return;
            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile, includeControllerXmlComments);
            }
        }

        /// <summary>
        /// Include xml file by assembly
        /// </summary>
        /// <param name="options"></param>
        /// <param name="assembly"></param>
        /// <param name="includeControllerXmlComments">
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </param>
        public static void IncludeXmlComments(this SwaggerGenOptions options,
            Assembly assembly, bool includeControllerXmlComments = false)
        {
            if (assembly == null || string.IsNullOrEmpty(assembly.Location)) return;

            var extensionDotIndex = assembly.Location.LastIndexOf('.');
            var path = assembly.Location.Substring(0, extensionDotIndex) + ".xml";

            if (!File.Exists(path)) return;
            options.IncludeXmlComments(path, includeControllerXmlComments);
        }

        public static void AddJwtBearerSecurity(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer", //The name of the previously defined security scheme.
                            Type = ReferenceType.SecurityScheme,
                        },
                        Name = "Bearer"
                    },
                    new string[] { }
                }
            });
        }
    }
}
