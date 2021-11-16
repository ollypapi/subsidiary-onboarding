using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Common.Models.StatementServive;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetOccupationQuery :  IRequest<ResponseModel<OccupationResponse>>
    {
        public string CountryCode { get; set; }
    }

    public class GetNTransactionsQueryHandler : IRequestHandler<GetOccupationQuery, ResponseModel<OccupationResponse>>
    {
        private readonly IFIService _fIService;
        public GetNTransactionsQueryHandler(IFIService fIService)
        {
            _fIService = fIService;
        }
        public async Task<ResponseModel<OccupationResponse>> Handle(GetOccupationQuery request, CancellationToken cancellationToken)
        {
            var data = await _fIService.GetOccupations(request.CountryCode);
            return ResponseModel<OccupationResponse>.Success(data);
        }
    }
}
