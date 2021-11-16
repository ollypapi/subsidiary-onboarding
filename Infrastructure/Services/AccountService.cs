using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Extentions;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccountService: IAccountService
    {

        private readonly IConfiguration _configuration;
        private readonly HttpClientHelper _httpClient;
        private string appId = string.Empty;
        private string appKey = string.Empty;
        private string baseUrl = string.Empty;
        public AccountService(HttpClientHelper httpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            appId = _configuration.GetValue<string>("FISetting:AppId");
            appKey = _configuration.GetValue<string>("FISetting:AppKey");
            baseUrl = _configuration.GetValue<string>("FISetting:BaseUrl");
        }

        

        public async Task<AccountCreationResponse> CreateAccount(AccountCreationRequest request)
        {
            try
            {
                Log.Information(JsonConvert.SerializeObject(request));
                var req = Initialize($"account/create-account");
               return await _httpClient.PostClient<AccountCreationResponse, AccountCreationRequest>(request, req, baseUrl);
            }
            catch (Exception ex)
            {
                var errorCode = ResponseCodeEnum.AccountCreationFailed.GetDescription();
                var message = "Account Creation failed...";
                Log.Error(ex, errorCode);
                throw new CustomException(message, errorCode);
            }
        }

        public async Task<AccountDetailResponse> GetAccountDetail(AccountDetailRequest request)
        {
            try
            {
                Log.Information(JsonConvert.SerializeObject(request));
                var req = Initialize($"account/get-account-details");
                return await _httpClient.PostClient<AccountDetailResponse, AccountDetailRequest>(request, req, baseUrl);
            }
            catch (Exception ex)
            {
                var errorCode = ResponseCodeEnum.GetAccountFailed.GetDescription();
                var message = "Get Account  failed...";
                Log.Error(ex, errorCode);
                throw new CustomException(message, errorCode);
            }
        }

       

        public async Task<PNDResponse> PlacePNDOnAccount(PNDRequest request)
        {
            try
            {
                Log.Information(JsonConvert.SerializeObject(request));
                var req = Initialize($"account/place-post-no-debit");
                return await _httpClient.PostClient<PNDResponse, PNDRequest>(request, req, baseUrl);
            }
            catch (Exception ex)
            {
                var errorCode = ResponseCodeEnum.AccountCreationFailed.GetDescription();
                var message = "Place PND failed";
                Log.Error(ex, errorCode);
                throw new CustomException(message, errorCode);
            }
        }

        public async Task<PNDResponse> RemovePNDOnAccount(PNDRequest request)
        {
            try
            {
                Log.Information(JsonConvert.SerializeObject(request));
                var req = Initialize($"account/remove-post-no-debit");
                return await _httpClient.PostClient<PNDResponse, PNDRequest>(request, req, baseUrl);
            }
            catch (Exception ex)
            {
                var errorCode = ResponseCodeEnum.AccountCreationFailed.GetDescription();
                var message = "Remove PND failed";
                Log.Error(ex, errorCode);
                throw new CustomException(message, errorCode);
            }
        }

       
        private RestRequest Initialize(string action)
        {

            var req = new RestRequest(action, Method.POST);
            req.AddHeader("AppId", appId);
            req.AddHeader("AppKey", appKey);
            req.RequestFormat = DataFormat.Json;
            return req;
        }
      
    }
}
