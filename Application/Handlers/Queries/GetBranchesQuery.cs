using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetBranchesQuery: IRequest<ResponseModel<BranchInfo>>
    {
        [JsonIgnore]
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
    }
    public class GetBranchesQueryValidator : AbstractValidator<GetBranchesQuery>
    {
        public GetBranchesQueryValidator()
        {
            RuleFor(c => c.StateCode).NotEmpty();
        }
    }
    public class GetBranchesQueryHandler : IRequestHandler<GetBranchesQuery, ResponseModel<BranchInfo>>
    {
        private readonly IFIService _fiService;
        public GetBranchesQueryHandler(IFIService fIService)
        {
            _fiService = fIService;
        }
        public async Task<ResponseModel<BranchInfo>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
        {
            var data = await _fiService.GetBranches(request.StateCode);
            return ResponseModel<BranchInfo>.Success(data);
        }
    }
}
