using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Trsys.Events.Abstractions;
using Trsys.Events.Infrastructure;

namespace Trsys.Events
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services, IEnumerable<Type> handlerTypes)
        {
            services.Configure<EventsOptions>(options =>
            {
                options.EventHandlerTypes = handlerTypes;
            });
            return services;
        }
    }
}
