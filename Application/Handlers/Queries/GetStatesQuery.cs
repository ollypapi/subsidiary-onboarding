using Application.Common.Models;
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
    public class GetStatesQuery:IRequest<ResponseModel<StateModel>>
    {
        public string CountryCode { get; set; }
    }
    public class GetStatesQueryHandler : IRequestHandler<GetStatesQuery, ResponseModel<StateModel>>
    {
        private readonly IFIService _fiService;
        public GetStatesQueryHandler(IFIService fIService)
        {
            _fiService = fIService;
        }
        public async Task<ResponseModel<StateModel>> Handle(GetStatesQuery request, CancellationToken cancellationToken)
        {
            var data = await _fiService.GetAllStates(request.CountryCode);
            return ResponseModel<StateModel>.Success(data);
        }
    }
}
