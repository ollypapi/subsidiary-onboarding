using System;
using System.Text.Json.Serialization;
using Application.Common.Behaviours;
using Application.Extentions;
using Application.Handlers.Commands.CustomerCommands;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.ServiceBase;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Presentation.Filters;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace Presentation
{
    public class Startup
    {
        public Startup(IHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var elasticUri = Configuration["ElasticConfiguration:Uri"];

            var logFile = "Onboarding-Log-";//$"{Assembly.GetEntryAssembly().FullName}".Replace(".", "_");
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext().Enrich.WithExceptionDetails().Enrich.WithMachineName().WriteTo.File($"logs/{logFile}", rollingInterval: RollingInterval.Hour).WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "Onboarding-{0:yyyy.MM.dd}"
                })
            .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCqrs();
            services.AddSwagger();
            services.AddServices();
            services.AddJwtAuthentication(Configuration);
            services.AddDependency();
            var conString = Configuration.GetValue<string>("DefaultConnection");
            var cacheString = Configuration.GetValue<string>("CacheConnection");
            services.AddDbContext<IOnboardingDbContext, AppDbContext>(options =>
                                             options.UseSqlServer(conString));

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = cacheString;
                options.SchemaName = "dbo";
                options.TableName = "OnboardingCache";
            });

            services.AddControllers(options => options.Filters.Add(new ApiExceptionFilter()))
                 .AddNewtonsoftJson()
                 .AddFluentValidation(fv =>
                 {
                     fv.RegisterValidatorsFromAssemblyContaining<InitiateOnboardingCommand>();
                 })
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
                
                
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("../swagger/v1/swagger.json", "Onboarding V1"); });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
