using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Interfaces;
using Infrastructure.ServiceBase;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExternalService : IExternalServiceInterface
    {
        private readonly HttpClientHelper _httpClientHelper;
        private readonly IConfiguration _configuration;
        private readonly BaseUtility _baseUtility;
        public ExternalService(HttpClientHelper httpClientHelper,BaseUtility baseUtility,
            IConfiguration configuration)
        {
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            _baseUtility = baseUtility;
        }
        public async Task<MoveFileResponse> MoveFiles(MoveFilesModel fileDetails)
        {
            Log.Information(JsonConvert.SerializeObject(fileDetails));
            var url = $"{_configuration.GetValue<string>("Util:FileManagerBase")}/api/FileManager/MoveFiles";

            var response = await _httpClientHelper.PostAsync<MoveFileResponse, MoveFilesModel>(fileDetails, url, null, null) ;
            return response;
        }

        public async Task<BranchInfo> GetBranches(string stateCode)
        {
            Log.Information("Get State Branches");
            var header = new Dictionary<string, string>();
            
            var url = $"{_baseUtility.BaseUrl}enquiry/get-branches-by-state-code";
            header.Add("AppId", _baseUtility.AppId);
            header.Add("AppKey", _baseUtility.AppId);
            var fiRequest = new BranchModel { CountryId=stateCode};
            var response = await _httpClientHelper.PostAsync<BranchInfo, BranchModel>(fiRequest, url, null, header);
            return response;
        }
        public async Task<MeansOfIdModel> GetMeansOfIdentification()
        {
            //  var url = "/api/v1/enquiry/get-nationalities";
            var header = new Dictionary<string, string>();

            var url = $"{_baseUtility.BaseUrl}enquiry/get-identification-categories";
            header.Add("AppId", _baseUtility.AppId);
            header.Add("AppKey", _baseUtility.AppKey);
            var fiRequest = new FIBaseRequest { };
            var response = await _httpClientHelper.PostAsync<MeansOfIdModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

        public async Task<NationalityModel> GetNationalities()
        {
          //  var url = "/api/v1/enquiry/get-nationalities";
            var header = new Dictionary<string, string>();

            var url = $"{_baseUtility.BaseUrl}enquiry/get-nationalities";
            header.Add("AppId", _baseUtility.AppId);
            header.Add("AppKey", _baseUtility.AppKey);
            var fiRequest = new FIBaseRequest { };
            var response = await _httpClientHelper.PostAsync<NationalityModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

        public async Task<SalutationModel> GetSalutation()
        {
            var header = new Dictionary<string, string>();

            var url = $"{_baseUtility.BaseUrl}enquiry/get-salutations";
            header.Add("AppId", _baseUtility.AppId);
            header.Add("AppKey", _baseUtility.AppKey);
            var fiRequest = new FIBaseRequest { };
            var response = await _httpClientHelper.PostAsync<SalutationModel, FIBaseRequest>(fiRequest, url, null, header);
            return response;
        }

        public Task<List<GeneralResponse>> GetTown()
        {
            throw new NotImplementedException();
        }

        public async Task<MoveFileResponse> SaveFiles(SaveFilesModel fileDetail)
        {
            var url = $"{_configuration.GetValue<string>("FileManagerSetting:BaseUrl")}FileManager/SaveDocument";
            return await _httpClientHelper.PostAsync<MoveFileResponse, SaveFilesModel>(fileDetail, url, null, null);
        }
    }
}
