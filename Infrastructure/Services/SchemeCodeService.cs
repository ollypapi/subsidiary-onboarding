using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SchemeCodeService : ISchemeCodeService
    {
        private readonly HttpClientHelper _httpClientHelper;
        private readonly IConfiguration _configuration;

        public SchemeCodeService(IConfiguration configuration, HttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
        }
        public async Task<ResponseModel> AddSchemeCode(SchemeCode request)
        {
            var url = $"{_configuration["AccountManagement:BaseUrl"]}SchemeCode/countryId/{request.CountryCode}";
            return await _httpClientHelper.PostAsync<ResponseModel, SchemeCode>(request, url, null, null);
        }

        public async Task<ResponseModel<List<SchemeCode>>> GetCountrySchemeCodes(string CountryCode)
        {
            var url = $"{_configuration["AccountManagement:BaseUrl"]}SchemeCode/countryId/{CountryCode}";
            return await _httpClientHelper.GetAsync<ResponseModel<List<SchemeCode>>>(url, null, null);
        }

        public async Task<ResponseModel> UpdateSchemeCode(SchemeCode request)
        {
            var url = $"{_configuration["AccountManagement:BaseUrl"]}SchemeCode/update/countryId/{request.CountryCode}";
            return await _httpClientHelper.PostAsync<ResponseModel, SchemeCode>(request, url, null, null);
        }
    }
}
