using System.Text;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Netcool.Swashbuckle.AspNetCore
{
    public static class SwaggerUIOptionsExtensions
    {
        public static void InjectHeadContent(this SwaggerUIOptions options, string headContent)
        {
            var builder = new StringBuilder(options.HeadContent);
            builder.AppendLine(headContent);
            options.HeadContent = builder.ToString();
        }
    }
}
