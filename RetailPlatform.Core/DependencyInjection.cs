using Microsoft.Extensions.DependencyInjection;
using RetailPlatform.Core.Carts.Commands.Delete;
using RetailPlatform.Core.Carts.Commands.Upsert;
using RetailPlatform.Core.Carts.Queries;

namespace RetailPlatform.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUpsertCartCommandHandler, UpsertCartCommandHandler>();
            services.AddScoped<IDeleteCartCommandHandler, DeleteCartCommandHandler>();
            services.AddScoped<IDeleteCartItemCommandHandler, DeleteCartItemCommandHandler>();
            services.AddScoped<IGetCartQueryHandler, GetCartQueryHandler>();

            return services;
        }
    }
}
