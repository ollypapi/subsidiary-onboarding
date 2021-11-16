using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Common.Models.Response;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetAccountDetailQuery: AccountDetailRequest, IRequest<ResponseModel<AccountDetailResponse>>
    {

    }

    public class GetAccountDetailQueryHandler : IRequestHandler<GetAccountDetailQuery, ResponseModel<AccountDetailResponse>>
    {
        private readonly IAccountService _accountService;
        public GetAccountDetailQueryHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<ResponseModel<AccountDetailResponse>> Handle(GetAccountDetailQuery request, CancellationToken cancellationToken)
        {
            var data = await _accountService.GetAccountDetail(request);
            return ResponseModel<AccountDetailResponse>.Success(data);
        }
    }


}
