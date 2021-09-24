using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Trsys.Frontend.Application.Admin.Dashboard;

namespace Trsys.Frontend.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFrontendInfrastructure(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DashboardSearchRequest).Assembly);
            return services;
        }
    }
}
