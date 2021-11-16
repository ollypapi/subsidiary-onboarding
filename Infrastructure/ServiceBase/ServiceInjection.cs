using Application.Helper;
using Application.Implementation;
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Infrastructure.ServiceBase
{
   public static class ServiceInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
      
            services.AddTransient<IExternalServiceInterface, ExternalService>();
            services.AddTransient<ICacheOptionsProvider, CacheOptionsProvider>();
            services.AddTransient<ICacheProvider, DistributedCacheProvider>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IStatementService, StatementService>();
            services.AddTransient<IFIService, FIService>();
            services.AddTransient<ISchemeCodeService, SchemeCodeService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ICryptoResource, CryptoResource>();
            services.AddTransient<ISubsidiaryPVC, SubsidiaryPVC>();
            services.AddScoped(typeof(ActionFilterHelper));
            services.AddScoped(typeof(ActivityLogHelper));
            services.AddScoped(typeof(AppDbContext));
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
