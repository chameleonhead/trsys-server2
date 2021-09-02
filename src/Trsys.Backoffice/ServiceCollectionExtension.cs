using EventFlow;
using EventFlow.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Trsys.Backoffice.Abstractions;
using Trsys.Backoffice.Infrastructure;
using Trsys.Backoffice.Infrastructure.ReadModels;
using Trsys.Backoffice.ReadModels.Users;
using Trsys.Backoffice.WriteModels.Users;

namespace Trsys.Backoffice
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBackofficeInfrastructure(this IServiceCollection services)
        {
            services.AddMediatR(typeof(UserQueryHandler).Assembly, typeof(UserCommandHandler).Assembly);
            services.AddDbContext<TrsysBackofficeContext>(options => options.UseSqlite("Data Source=backoffice.db"));
            services.AddTransient<IUserStore, UserStore>();
            services.AddSingleton(EventFlowOptions.New
                .AddCommands(new[] { typeof(UserCreateCommand) })
                .AddEvents(new[] { typeof(UserCreatedEvent) })
                .AddCommandHandlers(typeof(UserCommandHandler))
                .CreateResolver());
            return services;
        }
    }
}
