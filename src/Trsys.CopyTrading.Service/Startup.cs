using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System;
using Trsys.CopyTrading.Application;
using Trsys.CopyTrading.Infrastructure;
using Trsys.Events;

namespace Trsys.CopyTrading.Service
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<IEaService, Application.EaService>();
            services.AddSingleton<CopyTradingEventHandler>();
            services.AddInMemoryEaServiceInfrastructure();
            services.AddEventHandlers(new[] { typeof(CopyTradingEventHandler) })
                .AddInMemoryEventInfrastructure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetSampler(new AlwaysOnSampler())
                .AddSource("MySource")
                .AddZipkinExporter(options =>
                {
                    options.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                })
                .Build();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.Use(async (context, next) =>
            {
                var tracer = tracerProvider.GetTracer("MySource");
                using var span = tracer.StartSpan(context.Request.Path, SpanKind.Server);
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    span.RecordException(ex);
                }
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EaService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
