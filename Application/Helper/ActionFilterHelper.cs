using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Application.Helper
{
    public class ActionFilterHelper
    {
        readonly IOnboardingDbContext appContext;
        public ActionFilterHelper(IOnboardingDbContext dbContext)
        {
            appContext = dbContext;
        }
        public void LogCustomerId(ActionExecutingContext context, UserActivity activity)
        {
            var hasCustomerId = context.HttpContext.Request.RouteValues.TryGetValue("customerId", out var customer_Id);
            if (hasCustomerId)
            {
                activity.CustomerId = customer_Id.ToString();
                var cust = appContext.Customers.FirstOrDefault(a => a.CustomerId.Equals(customer_Id.ToString()));
                if (cust == null)
                    activity.AccountNumber = "NULL";
                else
                    activity.AccountNumber = cust.AccountNumber;
            }
        }
        
        public void LogCustomerAccountNumber(ActionExecutingContext context, UserActivity activity)
        {
            var hasAccountNumber = context.HttpContext.Request.RouteValues.TryGetValue("accountnumber", out var account_number);
            if (hasAccountNumber)
            {
                activity.AccountNumber = account_number.ToString();
                var cust = appContext.Customers.FirstOrDefault(a => a.AccountNumber.Equals(account_number.ToString()));
                if (cust == null)
                    activity.CustomerId = "NULL";
                else
                    activity.CustomerId = cust.CustomerId;
            }
        }
    }
}
