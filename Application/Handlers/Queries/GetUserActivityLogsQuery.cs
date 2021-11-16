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
    public class GetUserActivityLogsQuery : IRequest<ResponseModel<List<UserActivityLogResponse>>>
    {}

    public class GetUserActivityLogsQueryHandler : IRequestHandler<GetUserActivityLogsQuery, ResponseModel<List<UserActivityLogResponse>>>
    {
        private readonly IOnboardingDbContext _onboardingDbContext;
        public GetUserActivityLogsQueryHandler(IOnboardingDbContext onboardingDbContext)
        {
            _onboardingDbContext = onboardingDbContext;
        }
        public async Task<ResponseModel<List<UserActivityLogResponse>>> Handle(GetUserActivityLogsQuery request, CancellationToken cancellationToken)
        {
            return ResponseModel<List<UserActivityLogResponse>>
                .Success(await _onboardingDbContext.UserActivities
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
