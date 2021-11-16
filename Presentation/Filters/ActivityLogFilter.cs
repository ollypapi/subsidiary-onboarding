using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Persistence;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Application.Helper;
using Microsoft.AspNetCore.Http.Features;

namespace Presentation.Filters
{
    public class ActivityLogFilter : IAsyncActionFilter
    {
        private readonly AppDbContext onboardingDbContext;
        private readonly ILogger<ActivityLogFilter> Logger;
        private readonly ActionFilterHelper helper;
        public string ActivityType { get; set; }
        public dynamic CustomerId { get; set; }
        public ActivityLogFilter(string activity, ILogger<ActivityLogFilter> logger, AppDbContext onboardingDbContext)
        {
            ActivityType = activity;
            this.Logger = logger;
            this.onboardingDbContext = onboardingDbContext;
            helper = new ActionFilterHelper(onboardingDbContext);
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Logger.LogInformation("Activity Logs begins");
            var iPAddress = Convert.ToString(Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .FirstOrDefault(i => i.AddressFamily.Equals(AddressFamily.InterNetwork)));
            var controllerName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

            try
                {
                    var activity = new UserActivity
                    {
                        Activity = ActivityType,
                        ControllerName = controllerName,
                        ActionName = actionName,
                        Path = UriHelper.GetDisplayUrl(context.HttpContext.Request),
                        IPAddress = iPAddress
                    };

                    if (activity == null)
                        context.Result = new BadRequestResult();

                    var result = await next();
                    if (result.Result is OkResult || result.Result is OkObjectResult)
                    {
                        activity.ActivityResult = "Successful";
                        activity.ResultDescription = $"Request call was successfull on {actionName}";
                    }
                    else
                    {
                        activity.ActivityResult = "Failed";
                        activity.ResultDescription = $"Request call failed on {actionName}";
                    }

                    helper.LogCustomerId(context, activity);
                    helper.LogCustomerAccountNumber(context, activity);

                    await onboardingDbContext.UserActivities.AddAsync(activity);
                    await onboardingDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                    throw;
                }

            Logger.LogInformation("Activity Logs ends");
        }
    }
}
