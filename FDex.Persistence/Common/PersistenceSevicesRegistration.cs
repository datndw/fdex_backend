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
            ServiceLifetime.Transient);
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ISwapRepository, SwapRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAddLiquidityRepository, AddLiquidityRepository>();
            services.AddTransient<IPositionRepository, PositionRepository>();

            return services;
        }
    }
}