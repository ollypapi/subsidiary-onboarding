using Application.Common.Models;
using Application.Common.Models.AccountService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountCreationResponse> CreateAccount(AccountCreationRequest request);
        Task<AccountDetailResponse> GetAccountDetail(AccountDetailRequest request);
        Task<PNDResponse> PlacePNDOnAccount(PNDRequest request);
        Task<PNDResponse> RemovePNDOnAccount(PNDRequest request);

    }
}
