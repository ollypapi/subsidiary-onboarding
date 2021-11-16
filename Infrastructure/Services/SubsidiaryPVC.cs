using Application.Common.Models.Requests.FIBServiceRequests;
using Application.Common.Models.Response.FIBServiceResponse;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SubsidiaryPVC : ISubsidiaryPVC
    {
        private readonly HttpClientHelper httpClientHelper;
        private readonly ILogger<SubsidiaryPVC> logger;
        private readonly IConfiguration configuration;
        private readonly Dictionary<string, string> header;

        public SubsidiaryPVC(ILogger<SubsidiaryPVC> logger, HttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            this.logger = logger;
            this.httpClientHelper = httpClientHelper;
            this.configuration = configuration;
            header = new Dictionary<string, string>
            {
                ["AppId"] = configuration["CardValidationSettings:AppId"],
                ["AppKey"] = configuration["CardValidationSettings:AppKey"],
            };
        }
        public async Task<CustomerAccountResponse> VerifyCardPIN(CustomerCardValidationRequest request)
        {
            var url = string.Concat(configuration["CardValidationSettings:BaseUrl"], 
                configuration["CardValidationSettings:EndPoints:VerifyCardPin"]);
            
            return await httpClientHelper.PostAsync<CustomerAccountResponse, CustomerCardValidationRequest>(
                new CustomerCardValidationRequest
                {
                    CountryId = request.CountryId,
                    AccountNumber = request.AccountNumber,
                    CardPan = request.CardPan,
                    Pin = request.Pin,
                    MobileNumber = request.MobileNumber
                }, url, null, header);
        }
    }
}
