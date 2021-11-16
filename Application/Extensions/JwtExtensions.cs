using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application.Extentions
{
    public static class JwtExtensions
    {


        public static long GetCustomerId(this IIdentity identity)
        {
            return long.Parse(GetClaimValue(identity, ClaimTypes.NameIdentifier));
            //return long.Parse(GetClaimValue(identity, "CustomerId"));
        }

        public static string GetEmail(this IIdentity identity)
        {
            return GetClaimValue(identity, ClaimTypes.Email);
        }
        public static string GetUsername(this IIdentity identity)
        {
            return GetClaimValue(identity, ClaimTypes.NameIdentifier);
        }
        public static string GetAuthenticationStamp(this IIdentity identity)
        {
            return GetClaimValue(identity, "AuthStamp");
        }
        public static long GetClientId(this IIdentity identity)
        {
            return long.Parse(GetClaimValue(identity, ClaimTypes.NameIdentifier));
        }


        private static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {
            var claimsList = new List<Claim>(claims);
            var claim = claimsList.Find(c => c.Type == claimType);
            return claim != null ? claim.Value : null;
        }

        private static string GetClaimValue(IIdentity identity, string claimType)
        {
            var claimIdentity = (ClaimsIdentity)identity;
            return claimIdentity.Claims.GetClaimValue(claimType);
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = Configuration.GetValue<bool>("Jwt:ValidateSigningKey"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:SecurityKey"))),
                    ValidateIssuer = Configuration.GetValue<bool>("Jwt:ValidateIssuer"),
                    ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
                    ValidateAudience = Configuration.GetValue<bool>("Jwt:ValidateAudience"),
                    ValidAudience = Configuration.GetValue<string>("Jwt:Audience"),
                    ValidateLifetime = Configuration.GetValue<bool>("Jwt:ValidateLifeTime"),
                    ClockSkew = TimeSpan.FromMinutes(Configuration.GetValue<int>("Jwt:DateToleranceMinutes")) //5 minute tolerance for the expiration date
                };
            });
            services.AddAuthorization();
            //services.AddPolicy("OnlyMobifinAccess", "AllowCoreBanking", "Mobifin");
            //services.AddPolicy("OnlyAgentAccess", "CustomerType", CustomerType.AGENT.ToString());
            return services;
        }
    }
}
