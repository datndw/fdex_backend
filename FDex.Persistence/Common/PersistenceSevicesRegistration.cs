using System;
using EntityFrameworkCore.Triggered;
using FDex.Application.Contracts.Persistence;
using FDex.Persistence.DbContexts;
using FDex.Persistence.Repositories;
using FDex.Persistence.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FDex.Persistence.Common
{
    public static class PersistenceSevicesRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FDexDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("FDexDB"), sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                    );
                    options.UseTriggers(triggerOptions =>
                    {
                        triggerOptions.AddTrigger<UserLevelTrigger>();
                    });
                });
            },
            ServiceLifetime.Scoped);
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISwapRepository, SwapRepository>();
            return services;
        }
    }
}