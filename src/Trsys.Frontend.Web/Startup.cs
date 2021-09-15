using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using Trsys.Backoffice;
using Trsys.CopyTrading;
using Trsys.CopyTrading.Application;
using Trsys.Events;
using Trsys.Frontend.Hubs;
using Trsys.Frontend.Web.Caching;
using Trsys.Frontend.Web.Formatters;

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
            services.AddBackofficeInfrastructure();
            if (Configuration.GetValue<string>("Trsys:CopyTradingEndpoint") == "InMemory")
            {
                services.AddEventHandlers(new[] { typeof(CopyTradingEventHandler) })
                    .AddInMemoryEventInfrastructure();
            }
            services.AddSingleton<CopyTradingCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                .AddSource("Trsys.Frontend.Web")
                .SetResourceBuilder(ResourceBuilder
                    .CreateDefault()
                    .AddService(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ?? "trsys-server"))
                .AddZipkinExporter(options =>
                {
                    options.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                })
                .Build();
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
            app.Use(async (context, next) =>
            {
                var tracer = tracerProvider.GetTracer("Trsys.Frontend.Web");
                using var span = tracer.StartSpan(context.Request.Path, SpanKind.Server);
                try
                {
                    span.SetAttribute("service.name", Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ?? "trsys-server");
                    span.SetAttribute("service.namespace", "Trsys.Frontend.Web");
                    span.SetAttribute("service.instance.id", Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") ?? "Unknown");
                    span.SetAttribute("service.version", "20210915");
                    span.SetAttribute("http.host", context.Request.Host.Value);
                    span.SetAttribute("http.method", context.Request.Method);
                    var builder = new UriBuilder();
                    builder.Scheme = context.Request.Scheme;
                    builder.Host = context.Request.Host.Host;
                    if (context.Request.Host.Port.HasValue)
                    {
                        builder.Port = context.Request.Host.Port.Value;
                    }
                    builder.Path = context.Request.Path;
                    builder.Query = context.Request.QueryString.ToString();
                    span.SetAttribute("http.url", builder.Uri.ToString());
                    await next();
                    span.SetAttribute("http.status_code", context.Response.StatusCode);
                }
                catch (Exception ex)
                {
                    span.RecordException(ex);
                }
            });
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
