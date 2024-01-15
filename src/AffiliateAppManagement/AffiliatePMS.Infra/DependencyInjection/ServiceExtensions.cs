using AffiliatePMS.Application.Common;
using AffiliatePMS.Application.Contracts;
using AffiliatePMS.Domain.Affiliates;
using AffiliatePMS.Domain.Common;
using AffiliatePMS.Infra.Persistence;
using AffiliatePMS.Infra.Persistence.Common;
using AffiliatePMS.Infra.Persistence.RealTime;
using AffiliatePMS.Infra.Persistence.Sql;
using AffiliatePMS.Infra.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AffiliatePMS.Infra.DependencyInjection;

public static class ServiceExtensions
{
    //create method that receive IServiceCollection
    public static IServiceCollection Initialize(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Application.ApplicationAssembly).Assembly);
        });

        //add Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdentifierService, HttpIdentifier>();
        services.AddScoped<IUnitOfWork, DbContextUnitOfWork>();

        //Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IRepository<Affiliate>, Repository<Affiliate>>();
        services.AddScoped<IAffiliateRepository, AffiliateRepository>();
        services.AddScoped<IRealTimeStatisRepository, RedisCacheRepository>();


        services.AddDbContext<APMSDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("APMSConnection")));

        Application.ApplicationAssembly.ConfigureMappers();

        return services;
    }
}