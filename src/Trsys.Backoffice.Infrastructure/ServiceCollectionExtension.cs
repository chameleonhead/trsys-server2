using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Trsys.Backoffice.Infrastructure.ReadModels;
using Trsys.Backoffice.ReadModels.Users;

namespace Trsys.Backoffice.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBackofficeInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<TrsysBackofficeContext>(options => options.UseSqlite("Data Source=backoffice.db"));
            services.AddTransient<IUserStore, UserStore>();
            return services;
        }
    }
}
