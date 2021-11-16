using Application.Common.Behaviours;
using Application.Helper;
using Application.Interfaces;
using AutoMapper;
using FluentValidation;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure.ServiceBase
{
    public static class ServiceLoader
    {

        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
           // services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<HttpClientHelper>();
            services.AddTransient<BaseUtility>();
            services.AddTransient<DocumentHelper>(); 
            services.AddTransient<CustomerHelper>();
            services.AddTransient<AccountHelper>();
            services.AddTransient<SecurityQuestionHelper>();
            services.AddTransient<TokenizationHelper>();
            services.AddTransient<ApplicationSettingHelper>();

            services.AddTransient<VerificationHelper>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            return services;
        }
    }
}
