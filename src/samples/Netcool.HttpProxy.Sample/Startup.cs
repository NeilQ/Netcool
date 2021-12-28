using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Netcool.HttpProxy.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddProxy(options =>
            {
                options.MessageHandler = new HttpClientHandler
                {
                    AllowAutoRedirect = false
                };

                options.PrepareRequest = (originalRequest, message) =>
                {
                    message.Headers.Add("X-Forwarded-Host", originalRequest.Host.Host);
                    return Task.FromResult(0);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.Map("/baidu", builder => { builder.RunProxy("https://www.baidu.com"); });
        }
    }
}
