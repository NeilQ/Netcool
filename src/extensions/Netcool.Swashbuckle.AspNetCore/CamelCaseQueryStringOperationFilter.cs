using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Netcool.Swashbuckle.AspNetCore
{
    public class CamelCaseQueryStringOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) return;
            foreach (var item in operation.Parameters)
            {
                item.Name = item.Name.Substring(0, 1).ToLower() + item.Name.Substring(1);
            }
        }
    }
}
