using Application.Common.Models;
using Application.Common.Models.Requests.FIBServiceRequests;
using Application.Common.Models.Response;
using Application.Common.Models.Response.FIBServiceResponse;
using Application.Interfaces;
using Infrastructure.ServiceBase;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FIService : IFIService
    {
        private readonly HttpClientHelper _httpClientHelper;
        private readonly IConfiguration _configuration;
        private readonly BaseUtility _baseUtility;
        private readonly Dictionary<string, string> header;
        public FIService(HttpClientHelper httpClientHelper ,BaseUtility baseUtility,
            IConfiguration configuration)
        {
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            _baseUtility = baseUtility;

            header = new Dictionary<string, string>
            {
                ["AppId"] = _baseUtility.AppId,
                ["AppKey"] = _baseUtility.AppKey
            };
        }
        public async Task<StateModel> GetAllStates(string countryCode)
        {
            var url =$"{_baseUtility.BaseUrl}enquiry/get-states";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response =await _httpClientHelper.PostAsync<StateModel, FIBaseRequest>(fiRequest,url,null,header);
            return response;
        }

        public async Task<BranchInfo> GetBranches(string countryCode)
        {
            var url = $"{_baseUtility.BaseUrl}enquiry/get-subsidiary-branches";
            var fiRequest = new BranchModel { CountryId= countryCode};
            var response = await _httpClientHelper.PostAsync<BranchInfo, BranchModel>(fiRequest, url, null, header);
            return response;
        }
        public async Task<MeansOfIdModel> GetMeansOfIdentification(string countryCode)
        {
            //  var url = "/api/v1/enquiry/get-nationalities";
            var url = $"{_baseUtility.BaseUrl}enquiry/get-retail-identification-types";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<MeansOfIdModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }
        public async Task<CityModel> GetCities(string countryCode)
        {
            //  var url = "/api/v1/enquiry/get-nationalities";
            var url = $"{_baseUtility.BaseUrl}enquiry/get-cities";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<CityModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }
        //enquiry/get-marital-statuses
        public async Task<MarritalStatusModel> GetMarritalStatus(string countryCode)
        {
            var url = $"{_baseUtility.BaseUrl}enquiry/get-marital-statuses";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<MarritalStatusModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }
        public async Task<NationalityModel> GetNationalities(string countryCode)
        {
            var url = $"{_baseUtility.BaseUrl}enquiry/get-nationalities";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<NationalityModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

        public async Task<SalutationModel> GetSalutation(string countryCode)
        {
            var url = $"{_baseUtility.BaseUrl}enquiry/get-salutations";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<SalutationModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

        public Task<List<GeneralResponse>> GetTown()
        {
            throw new NotImplementedException();
        }

        public async Task<SubsidiariesResponse> GetAllSusidiaries(string countryCode)
        {
            var url = $"{_baseUtility.BaseUrl}enquiry/get-subsidiaries";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<SubsidiariesResponse, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

        public async Task<OccupationResponse> GetOccupations(string countryCode)
        {
            var url = $"{_baseUtility.BaseUrl}enquiry/get-occupations";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<OccupationResponse, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

   
        public async Task<CountriesModel> GetCountriess(string countryCode)
        {
            var url = $"{_baseUtility.BaseUrl}enquiry/get-countries";
            var fiRequest = new FIBaseRequest { };
            fiRequest.CountryId = countryCode;
            var response = await _httpClientHelper.PostAsync<CountriesModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

        public async Task<CustomerByMobileResponse> GetCustomerByMobileNo(string countryCode, string mobileNumber)
        {
            var url = string.Concat(_baseUtility.BaseUrl, _configuration["FISetting:CutomerByMobileNo"]);
            return await _httpClientHelper.PostAsync<CustomerByMobileResponse, CustomerByMobileRequest>(
                new CustomerByMobileRequest 
                { 
                    CountryId = countryCode,
                    MobileNumber = mobileNumber
                }, url, null, header);
        }

        public async Task<CustomerAccountResponse> GetCustomerByAccountNo(string countryCode, string accountNumber)
        {
            var url = string.Concat(_baseUtility.BaseUrl, _configuration["FISetting:CustomerByAccountNo"]);
            return await _httpClientHelper.PostAsync<CustomerAccountResponse, CustomerByAccountNoRequest>(
                new CustomerByAccountNoRequest
                {
                    CountryId = countryCode,
                    AccountNumber = accountNumber
                }, url, null, header);
        }

        public async Task<AccountDetailsResponse> GetAccountDetails(string CountryCode, string AccountNumber)
        {
            var data = new AccountDetailByAccountNoRequest {CountryId = CountryCode, AccountNumber = AccountNumber };
            var url = string.Concat(_baseUtility.BaseUrl, _configuration["FISetting:AccountDetailsByAccountNumber"]);
            return await _httpClientHelper.PostAsync<AccountDetailsResponse, AccountDetailByAccountNoRequest>(data , url, null, header);
        }
    }
}
