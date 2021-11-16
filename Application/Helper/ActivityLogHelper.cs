using Application.Common.Models;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Application.Helper
{
    public class ActivityLogHelper
    {
        private readonly IOnboardingDbContext appContext;
        private readonly IHttpContextAccessor httpContext;
        public string ActivityResult { get; set; }
        public string ResultDescription { get; set; }
        public ActivityLogHelper(IOnboardingDbContext dbContext, IHttpContextAccessor context)
        {
            appContext = dbContext;
            httpContext = context;
        }
        public void LogCurrentActivity(string activityName, UserActivityRequestModel model)
        {
            var request = httpContext.HttpContext.Request;
            var iPAddress = Convert.ToString(Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .FirstOrDefault(i => i.AddressFamily.Equals(AddressFamily.InterNetwork)));
            var controllerName = httpContext.HttpContext.Request.RouteValues["controller"].ToString();
            var actionName = httpContext.HttpContext.Request.RouteValues["action"].ToString();

            var activity = new UserActivity();
            var cust = appContext.Customers
                .FirstOrDefault(a => a.AccountNumber.Equals(model.AccountNumber));

            if (cust != null)
            {
                activity.AccountNumber = cust.AccountNumber ?? model.AccountNumber;
                activity.CustomerId = cust.CustomerId ?? model.CustomerId;
            }
            else
            {
                activity.AccountNumber = "Null";
                activity.CustomerId = "Null";
            }

            activity.Activity = activityName;
            activity.ControllerName = controllerName;
            activity.ActionName = actionName;
            activity.Path = UriHelper.GetDisplayUrl(httpContext.HttpContext.Request);
            activity.IPAddress = iPAddress;
            activity.ActivityResult = ActivityResult;
            activity.ResultDescription = ResultDescription;

            appContext.UserActivities.Add(activity);
            appContext.SaveChanges();
        }
    }
}
