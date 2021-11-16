using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Models.StatementServive;
using Application.Extentions;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    class StatementService : IStatementService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClientHelper _httpClient;
        private string appId = string.Empty;
        private string appKey = string.Empty;
        private string baseUrl = string.Empty;
        public StatementService(HttpClientHelper httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            appId = _configuration.GetValue<string>("StatementSetting:AppId");
            appKey = _configuration.GetValue<string>("StatementSetting:AppKey");
            baseUrl = _configuration.GetValue<string>("StatementSetting:BaseUrl");
        }

        private RestRequest Initialize(string action)
        {

            var req = new RestRequest(action, Method.POST);
            req.AddHeader("AppId", appId);
            req.AddHeader("AppKey", appKey);
            req.RequestFormat = DataFormat.Json;
            return req;
        }

        public async Task<NTransactionResponse> GetNTransactions(GetNTransactionRequest request)
        {
            try
            {
                var req = Initialize($"statement/last-N-transactions");
                return await _httpClient.PostClient<NTransactionResponse, GetNTransactionRequest>(request, req, baseUrl);
            }
            catch (Exception ex)
            {
                var errorCode = ResponseCodeEnum.GetNTransactionFailed.GetDescription();
                var message = "Get N Transaction failed...";
                Log.Error(ex, errorCode);
                throw new CustomException(message, errorCode);
            }
        }
    }
}
