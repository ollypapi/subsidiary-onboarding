using Application.Common.Models;
using Application.Common.Models.Requests.FIBServiceRequests;
using Application.Common.Models.Response.FIBServiceResponse;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubsidiaryPVC
    {
        Task<CustomerAccountResponse> VerifyCardPIN(CustomerCardValidationRequest request);
    }
}
