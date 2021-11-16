using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Handlers.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [CustomAuthorize]
    public class UserActivityLogController : BaseController
    {
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<UserActivityLogResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("ActivityLogs")]
        public async Task<IActionResult> GetUserActivityLog()
            => Ok(await Mediator.Send(new GetUserActivityLogsQuery { }));


        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<UserActivityLogResponse>>), (int)HttpStatusCode.OK)]
        [HttpGet("ActivityLogs/{customerId}")]
        public async Task<IActionResult> GetUserActivityLog([FromRoute] long customerId)
            => Ok(await Mediator.Send(new GetUserActivityLogByCustomerIdQuery { CustomerId = customerId }));
    }
}
