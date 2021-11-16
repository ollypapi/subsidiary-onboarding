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
    public class GetSalutationQuery: IRequest<ResponseModel<SalutationModel>>
    {
        public string CountryCode { get; set; }
    }
    public class GetSalutationQueryHandler : IRequestHandler<GetSalutationQuery, ResponseModel<SalutationModel>>
    {
        private readonly IFIService _fiService;
        public GetSalutationQueryHandler(IFIService fIService)
        {
            _fiService = fIService;
        }
        public async Task<ResponseModel<SalutationModel>> Handle(GetSalutationQuery request, CancellationToken cancellationToken)
        {
            var data = await _fiService.GetSalutation(request.CountryCode);
            return ResponseModel<SalutationModel>.Success(data);
        }
    }
}