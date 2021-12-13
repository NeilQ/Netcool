using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Netcool.Core.Helpers;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Acartons.Api.Swagger
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
                    property.Value.Description += "</br>";
                    property.Value.Description += DescribeEnum(propertyEnums, property.Key);
                }
            }

            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths.Values)
            {
                DescribeEnumParameters(pathItem.Operations, swaggerDoc);
            }
        }

        private void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, OpenApiDocument swaggerDoc)
        {
            if (operations == null) return;
            foreach (var operation in operations)
            {
                foreach (var param in operation.Value.Parameters)
                {
                    var paramEnum = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == param.Name);
                    if (paramEnum.Value != null)
                    {
                        param.Description += DescribeEnum(paramEnum.Value.Enum, paramEnum.Key);
                    }
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
            var enumDescriptions = new List<string>();
            var enumType = GetEnumTypeByName(propertyTypeName);
            if (enumType == null)
                return null;

            foreach (var openApiAny in enums)
            {
                var enumOption = (OpenApiInteger)openApiAny;
                var enumInt = enumOption.Value;

                enumDescriptions.Add($"{enumInt} = {Reflection.GetEnumDescription(enumType, enumInt)}");
            }

            return string.Join("</br>", enumDescriptions.ToArray());
        }
    }
}
