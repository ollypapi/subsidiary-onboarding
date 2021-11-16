using Application.Helper;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace Persistence.Extension
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddMoreServices(this IServiceCollection services)
        {
            services.AddTransient<DocumentHelper>();
            services.AddTransient<CustomerHelper>();
             services.AddTransient<VerificationHelper>();
            return services;
        }
    }
}
