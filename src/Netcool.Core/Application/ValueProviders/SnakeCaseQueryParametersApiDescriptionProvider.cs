using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Netcool.Core.Extensions;

namespace Acartons.Parking.Core.Api.ValueProviders
{
    public class SnakeCaseQueryParametersApiDescriptionProvider : IApiDescriptionProvider
    {
        public int Order => 1;

        public void OnProvidersExecuted(ApiDescriptionProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApiDescriptionProviderContext context)
        {
            foreach (var parameter in context.Results.SelectMany(x => x.ParameterDescriptions).Where(x => x.Source.Id == "Query" || x.Source.Id == "ModelBinding"))
            {
                parameter.Name = parameter.Name.ToSnakeCase();
            }
        }
    }
}