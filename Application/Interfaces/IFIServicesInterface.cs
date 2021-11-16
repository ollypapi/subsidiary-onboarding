using Application.Common.Models.Response;
using Application.Common.Models.Response.FIBServiceResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFIService
    {
        Task<StateModel> GetAllStates(string countryCode);
        Task<SubsidiariesResponse> GetAllSusidiaries(string countryCode);
        Task<BranchInfo> GetBranches(string stateCode);
        Task<SalutationModel> GetSalutation(string countryCode);
        Task<List<GeneralResponse>> GetTown();
        Task<MarritalStatusModel> GetMarritalStatus(string countryCode);
        Task<MeansOfIdModel> GetMeansOfIdentification(string countryCode);
        Task<NationalityModel> GetNationalities(string countryCode);
        Task<CityModel> GetCities(string countryCode);
        Task<OccupationResponse> GetOccupations(string countryCode);
        Task<CountriesModel> GetCountriess(string countryCode);
        Task<CustomerByMobileResponse> GetCustomerByMobileNo(string countryCode, string mobileNumber);
        Task<CustomerAccountResponse> GetCustomerByAccountNo(string countryCode, string accountNumber);
        Task<AccountDetailsResponse> GetAccountDetails(string CountryCode, string AccountNumber);

    }
}
