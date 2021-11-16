using Application.Common.Models;
using Application.Interfaces;
using Infrastructure.ServiceBase;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly BaseUtility _baseUtility;
        private readonly HttpClientHelper _httpClient;

        public NotificationService(IConfiguration configuration, HttpClientHelper httpClientHelper, BaseUtility baseUtility)
        {
           
            _httpClient = httpClientHelper;
            _baseUtility = baseUtility;
        }

        public async Task<ResponseModel> SendMail(SendMailModel request)
        {
            try
            {
                if (request != null)
                {
                    request.AppCode = _baseUtility.EmailServiceAppKey;
                    var endpoint = $"{_baseUtility.EmailServiceBaseUrl}EmailService/SendMail";
                    var response = await _httpClient.PostAsync<ResponseModel, SendMailModel>(request, endpoint, null, null);
                    return response;
                }
                return ResponseModel.Failure("Unsuccesful..");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Email error");
                throw ex;
            }
        }
    }
}
