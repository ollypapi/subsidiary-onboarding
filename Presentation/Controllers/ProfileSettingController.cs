using System.Net;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
using Application.Handlers.Commands.CustomerCommands;
using Application.Handlers.Commands.OTPCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileSettingController : BaseController
    {
       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Transaction PIN creation" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("SetTransactionPin"),CustomAuthorize]
        public async Task<IActionResult> CreateTransactionPin( [FromBody] SetTransactionPinCommand command)
        {
            command.CustomerId = User.Identity.GetCustomerId();
            var res = await Mediator.Send(command);
            return Ok(res);
        }


        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Create password" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("CreatePassword"), CustomAuthorize]
        public async Task<IActionResult> CreatePassword([FromBody] SetPasswordCommand command)
        {
            command.CustomerId = User.Identity.GetCustomerId();
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Initialize password reset" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("InitiatePasswordReset")]
        public async Task<IActionResult> InitiatePasswordReset(  [FromBody] InitiateResetPasswordOTPCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Password reset" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(  [FromBody] ResetPasswordCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Change PIN" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [CustomAuthorize]
        [HttpPost, Route("ChangePassword"), CustomAuthorize]
        public async Task<IActionResult> ChangePin(  [FromBody] ChangePasswordCommand command)
        {
            command.CustomerId = User.Identity.GetCustomerId();
            var res = await Mediator.Send(command);
            return Ok(res);
        }

      
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Initialize PIN reset" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("InitiatePinReset"), CustomAuthorize]
        public async Task<IActionResult> InitiatePinReset([FromBody] InitiateResetPinOTPCommand command)
        {

            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "PIN reset" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [CustomAuthorize]
        [HttpPost, Route("ResetPin"), CustomAuthorize]
        public async Task<IActionResult> ResetPin( [FromBody] ResetPinCommand command)
        {
            command.CustomerId = User.Identity.GetCustomerId();
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Change PIN" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("ChangeTransactionPin"), CustomAuthorize]
        public async Task<IActionResult> ChangePin( [FromBody] ChangeTransactionPinCommand command)
        {
            command.CustomerId = User.Identity.GetCustomerId();
            var res = await Mediator.Send(command);
            return Ok(res);
        }

    }
}