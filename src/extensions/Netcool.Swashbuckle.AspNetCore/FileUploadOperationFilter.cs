using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Netcool.Api
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var isFileUploadOperation =
                context.MethodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(FileContentType));

            if (!isFileUploadOperation) return;

            operation.Parameters.Clear();
            operation.Parameters.Insert(0, new OpenApiParameter()
            {
                Name = "customFilename",
                Schema = new OpenApiSchema()
                {
                    Type = "boolean"
                },
                In = ParameterLocation.Query,
                Description = "Custom filename",
            });

            var uploadFileMediaType = new OpenApiMediaType()
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties =
                    {
                        ["uploadedFile"] = new OpenApiSchema
                        {
                            Description = "Uploaded File",
                            Type = "file",
                            Format = "formData"
                        }
                    },
                    Required = new HashSet<string> {"uploadedFile"}
                }
            };

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = {["multipart/form-data"] = uploadFileMediaType}
            };
        }

        [AttributeUsage(AttributeTargets.Method)]
        public class FileContentType : Attribute
        {
        }
    }
}
