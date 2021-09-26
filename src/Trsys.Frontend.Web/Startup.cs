using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using Trsys.Backoffice;
using Trsys.CopyTrading;
using Trsys.Frontend.Infrastructure;
using Trsys.Frontend.Web.Formatters;
using Trsys.Frontend.Web.Hubs;

namespace Trsys.Frontend.Web
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
            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Add(new TextPlainInputFormatter());
            });
            services.AddSignalR();

            services.AddEaService(options =>
            {
                options.ServiceEndpoint = Configuration.GetValue<string>("Trsys:CopyTradingEndpoint");
            });

            services.AddFrontendInfrastructure();

            services.AddBackofficeInfrastructure();
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ?? "trsys-server"))
                    .AddAspNetCoreInstrumentation()
                    .AddSource("Trsys.CopyTrading")
                    .AddZipkinExporter(options =>
                    {
                        var endpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_ZIPKIN_ENDPOINT");
                        if (!string.IsNullOrEmpty(endpoint))
                        {
                            options.Endpoint = new Uri(endpoint);
                        }
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CopyTradingHub>("/copyTradingHub");
            });
        }
    }
}
