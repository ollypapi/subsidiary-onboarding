using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetMeansOfIdentificationQuery : IRequest<ResponseModel<SalutationModel>>
    {
        [JsonIgnore]
        public string CountryCode { get; set; }
    }
    public class GetMeansOfIdentificationQueryHandler : IRequestHandler<GetMeansOfIdentificationQuery, ResponseModel<SalutationModel>>
    {
        private readonly IFIService _fiService;
        public GetMeansOfIdentificationQueryHandler(IFIService fIService)
        {
            _fiService = fIService;
        }
        public async Task<ResponseModel<SalutationModel>> Handle(GetMeansOfIdentificationQuery request, CancellationToken cancellationToken)
        {
            var data = await _fiService.GetSalutation(request.CountryCode);
            return  ResponseModel<SalutationModel>.Success(data);
        }
    }
}
