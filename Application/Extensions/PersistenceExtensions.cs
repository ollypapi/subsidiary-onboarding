using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;

namespace Application.Extentions
{
    public static class PersistenceExtensions
    {

        //public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        //{

        //    var conString = configuration.GetValue<string>("DefaultConnection");
        //    services.AddDbContext<INotificationDbContext, AppDbContext>(options => options.UseSqlServer(conString, b => b.MigrationsAssembly("MobilityOnboarding.API")));
        //    return services;
        //}

    }
}
