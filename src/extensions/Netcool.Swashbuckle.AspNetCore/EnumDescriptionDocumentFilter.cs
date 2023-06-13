using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Netcool.Swashbuckle.AspNetCore
{
    public class EnumDescriptionDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // add enum descriptions to result models
            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                var propertyEnums = property.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    property.Value.Description += "<p>Members:</p>";
                    property.Value.Description += DescribeEnum(propertyEnums, property.Key);
                }
            }

            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                DescribeEnumParameters(pathItem.Operations, context);
            }
        }

        private void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, DocumentFilterContext context)
        {
            if (operations == null) return;
            foreach (var operation in operations)
            {
                foreach (var param in operation.Value.Parameters)
                {
                    var schemaReferenceId = param.Schema.Reference?.Id;
                    if (string.IsNullOrEmpty(schemaReferenceId)) continue;

                    var schema = context.SchemaRepository.Schemas[schemaReferenceId];
                    if (schema.Enum == null || schema.Enum.Count == 0) continue;

                    param.Description += schema.Description;
                }
            }
        }

        private Type GetEnumTypeByName(string enumTypeName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.Name == enumTypeName);
        }

        private string DescribeEnum(IEnumerable<IOpenApiAny> enums, string propertyTypeName)
        {
            var enumType = GetEnumTypeByName(propertyTypeName);
            if (enumType == null)
                return null;

            var des = new StringBuilder();
            
            des.Append("<ul>");
            foreach (var openApiAny in enums)
            {
                var enumOption = (OpenApiInteger)openApiAny;
                var enumInt = enumOption.Value;

                des.Append($"<li>{enumInt} = {GetEnumDescription(enumType, enumInt)}</li>");
            }

            return des.ToString();
        }

        private string GetEnumDescription(Type type, int value)
        {
            var name = Enum.GetName(type, value);
            if (name == null) return null;
            var field = type.GetField(name);
            if (field == null) return null;
            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            return attr == null ? field.Name : attr.Description;
        }
    }
}
