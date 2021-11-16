using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetUserActivityLogByCustomerIdQuery : IRequest<ResponseModel<List<UserActivityLogResponse>>>
    {
        public long CustomerId { get; set; }
    }

    public class GetUserActivityLogByCustomerIdQueryHandler : IRequestHandler<GetUserActivityLogByCustomerIdQuery, ResponseModel<List<UserActivityLogResponse>>>
    {
        private readonly IOnboardingDbContext _onboardingDbContext;
        public GetUserActivityLogByCustomerIdQueryHandler(IOnboardingDbContext onboardingDbContext)
        {
            _onboardingDbContext = onboardingDbContext;
        }
        public async Task<ResponseModel<List<UserActivityLogResponse>>> Handle(GetUserActivityLogByCustomerIdQuery request, CancellationToken cancellationToken)
        {
            return ResponseModel<List<UserActivityLogResponse>>
                .Success(await _onboardingDbContext.UserActivities
                .Where(c => c.CustomerId.Equals(request.CustomerId))
                .OrderByDescending(x => x.TimeStamp)
                .Select(u => new UserActivityLogResponse
                {
                    Id = u.Id,
                    CustomerId = u.CustomerId,
                    AccountNumber = u.AccountNumber,
                    Activity = u.Activity,
                    ActivityResult = u.ActivityResult,
                    ResultDescription = u.ResultDescription,
                    ControllerName = u.ControllerName,
                    ActionName = u.ActionName,
                    Path = u.Path,
                    IPAddress = u.IPAddress,
                    ActivityTrackerId = u.ActivityTrackerId,
                    TimeStamp = u.TimeStamp
                }).ToListAsync(), "Record found.");
        }
    }
}
