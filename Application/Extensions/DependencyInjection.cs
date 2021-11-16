using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Behaviours;

namespace Application.Extensions
{
    public static class DependencyInjection
    {
        //public static iservicecollection addapplication(this iservicecollection services)
        //{
        //    services.addautomapper(assembly.getexecutingassembly());
        //    services.addvalidatorsfromassembly(assembly.getexecutingassembly());
        //    services.addmediatr(assembly.getexecutingassembly());
        //    services.addtransient<httpclienthelper>();
        //    services.addtransient<baseutility>();
        //    services.addtransient(typeof(ipipelinebehavior<,>), typeof(performancebehaviour<,>));
        //    services.addtransient(typeof(ipipelinebehavior<,>), typeof(validationbehavior<,>));
        //    services.addtransient(typeof(ipipelinebehavior<,>), typeof(unhandledexceptionbehaviour<,>));


        //    return services;
        //}
    }
}
