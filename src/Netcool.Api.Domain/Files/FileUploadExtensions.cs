using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Netcool.Api.Domain.Files
{
    public static class FileUploadExtensions
    {
        public static void UseUploadedStaticFiles(this IApplicationBuilder app, FileUploadOptions fileUploadOptions,
            ILogger logger)
        {
            if (fileUploadOptions == null) return;
            var path = fileUploadOptions.SubWebPath;

            if (string.IsNullOrWhiteSpace(fileUploadOptions.PhysicalPath))
            {
                logger.LogWarning("文件物理目录未配置");
            }
            else
            {
                if (!Directory.Exists(fileUploadOptions.PhysicalPath))
                {
                    try
                    {
                        Directory.CreateDirectory(fileUploadOptions.PhysicalPath);
                        logger.LogWarning($"文件保存物理路径不存在，已自动创建目录：{fileUploadOptions.PhysicalPath}");
                    }
                    catch (Exception e)
                    {
                        logger.LogWarning(e, $"文件保存物理路径不存在，试图创建失败: {fileUploadOptions.PhysicalPath}");
                        return;
                    }
                }

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(fileUploadOptions.PhysicalPath),
                    RequestPath = new PathString($"/{path.Trim('/')}")
                });
            }
        }
    }
}