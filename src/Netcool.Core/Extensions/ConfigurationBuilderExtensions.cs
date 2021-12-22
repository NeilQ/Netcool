using System.IO;
using Microsoft.Extensions.Configuration;

namespace Netcool.Core;

public static class ConfigurationBuilderExtensions
{
    public static void AddJsonFileFromDirectory(this IConfigurationBuilder configBuilder, string directoryPath)
    {
        if (!Directory.Exists(directoryPath)) return;

        var files = Directory.GetFiles(directoryPath);
        if (files.Length <= 0) return;

        foreach (var file in files)
        {
            if (Path.GetExtension(file) == ".json")
            {
                configBuilder.AddJsonFile(file, optional: true, reloadOnChange: true);
            }
        }
    }
}
