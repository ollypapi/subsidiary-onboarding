using System.Net;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Handlers.Commands.CustomerCommands;
using Application.Handlers.Commands.OTPCommands;
using Application.Handlers.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
      
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<LoginResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("SignIn")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Security question" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<SecurityQuestionResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("security-question/accountnumber/{accountnumber}/country/{countryCode}")]
        public async Task<IActionResult> GetSecurityQuestion( [FromRoute] string accountnumber, [FromRoute] string countryCode)
        {
            var command = new GetSecurityQuestionQuery { AccountNumber = accountnumber, CountryCode = countryCode };
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       /* [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Security question" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<SecurityQuestionResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("unsigned/security-question/accountnumber/{accountnumber}")]
        public async Task<IActionResult> UnsignedGetSecurityQuestion([FromRoute] string accountnumber)
        {
            var command = new GetSecurityQuestionQuery { AccountNumber = accountnumber };
            var res = await Mediator.Send(command);
            return Ok(res);
        } */

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Security question answer verification" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<SecurityQuestionAnswerValidationResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("security-question/verify")]
        public async Task<IActionResult> VerifySecurityQuestionAnswer([FromBody] ValidateSecurityQuestionAnswerCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       /* [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Security question answer verification" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<SecurityQuestionAnswerValidationResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("unsigned/security-question/verify")]
        public async Task<IActionResult> VerifyUnsignedSecurityQuestionAnswer([FromBody] ValidateSecurityQuestionAnswerCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        } */



        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Otp initialization" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("InitiateOTP")]
        public async Task<IActionResult> InitiateOTP( [FromBody] InitiateOTPCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Verify Otp on device" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("verifyOTP/add-device")]
        public async Task<IActionResult> VerifyOTPToAddDevice([FromBody] VerifyOtpToAddDeviceCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }


        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "PIN verification" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("verifyPin"), CustomAuthorize]
        public async Task<IActionResult> VerifyPin( [FromBody] VerifyPinCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Logout" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }


        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<LoginResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("refreshtoken"), CustomAuthorize]
        public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
    }
}