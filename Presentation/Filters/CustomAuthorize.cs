using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Filters
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {

        public CustomAuthorize()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var user = context.HttpContext.User;

            var isAuthenticated = user.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                string appToken = context.HttpContext.Request.Headers["AppKey"];
                string appId = context.HttpContext.Request.Headers["AppId"];
                if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appToken))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            /*  if (!user.Identity.IsAuthenticated && (string.IsNullOrEmpty(appToken) || string.IsNullOrEmpty(appId)))
              {
                  // it isn't needed to set unauthorized result 
                  // as the base class already requires the user to be authenticated
                  // this also makes redirect to a login page work properly
                  // context.Result = new UnauthorizedResult();
                  return;
              } */
          //  context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.OK);
           

        }
    }
}
