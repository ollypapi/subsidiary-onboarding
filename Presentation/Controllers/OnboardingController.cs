using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Common.Models.Response;
using Application.Common.Models.Response.FIBServiceResponse;
using Application.Extentions;
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
    public class OnboardingController : BaseController
    {
       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Initiate onboarding" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("CreateCustomerProfile")]
        public async Task<IActionResult> InitiateOnboarding([FromBody] InitiateOnboardingCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

      
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Otp initialization" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("InitiateOTP")]
        public async Task<IActionResult> InitiateOTP([FromBody] InitiateOTPCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Otp retry" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("ResendOTP")]
        public async Task<IActionResult> ResendOTP( [FromBody] ResendOTPCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Otp verify" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<Token>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp( [FromBody] VerifyOtpCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Get region" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<StateModel>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("GetAllRegions/countryId/{countryId}")]
        public async Task<IActionResult> GetRegion([FromRoute] string countryId)
        {
            var command = new GetStatesQuery {CountryCode = countryId };
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Combine model" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<CombinedModel>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("GetOnboardingDetails/countryId/{countryId}")]
        public async Task<IActionResult> GetCombinedModel([FromRoute] string countryId)
        {
            var command = new GetCombinedDetailsQuery { };
            command.CountryCode = countryId;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Get branch" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<BranchInfo>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("GetBranches/statecode/{stateCode}")]
        public async Task<IActionResult> GetBranch([FromRoute]string stateCode)
        {
            var command = new GetBranchesQuery {StateCode=stateCode };
            var res = await Mediator.Send(command);
            return Ok(res);
        }


        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Get salutation" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<GeneralResponse>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("GetSalutations/countryId/{countryId}")]
        public async Task<IActionResult> GetSalutation([FromRoute] string countryId)
        {
            var command = new GetSalutationQuery { CountryCode = countryId };
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Security question" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<GeneralResponse>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("SetSecurityQuestion"), CustomAuthorize]
        public async Task<IActionResult> PostSecurityQuestion( [FromBody] SetSecurityQuestionCommand command)
        {
            command.CustomerId = User.Identity.GetCustomerId();
            var res = await Mediator.Send(command);
            return Ok(res);
        }

      
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Save documents" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<GeneralResponse>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("SaveDocuments")]
        public async Task<IActionResult> PostSaveDocuments([FromBody] SaveDocumentCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Onboarding resumption" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OnboardingPriorityResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("ResumeOnboarding")]
        public async Task<IActionResult> ResumeOnboarding( [FromBody] ResumeOnboardingCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Onboarding completion" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<AccountCreationResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("complete")]
        public async Task<IActionResult> CompleteOnboarding( [FromBody] FinalizeOnboardingCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

      
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Onboarding existing customer" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<CustomerAccountResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost("ExistingCustomer/country/{countrycode}")]
        public async Task<IActionResult> OnboardExistingCustomer( [FromRoute] string countrycode, 
            [FromBody] OnboardExistingCustomerCommand command)
        {
            command.CountryCode = countrycode;
            return Ok(await Mediator.Send(command));
        }

       
      /*  [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Onboarding existing customer" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost("initiate/ExistingCustomer/country/{countrycode}")]
        public async Task<IActionResult> InitiateOnboardExistingCustomer( [FromRoute] string countrycode, [FromBody] InitiateOnboardingExistingCustomerCommand command)
        {
            command.CountryCode = countrycode;
            return Ok(await Mediator.Send(command));
        } */

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Onboarding existing customer with card" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<CustomerAccountResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost("ExistingCustomerViaCard/country/{countrycode}")]
        public async Task<IActionResult> OnboardExistingCustomerWithCard([FromRoute] string countrycode,
           [FromBody] OnboardExistingCustomerWithCardCommand command)
        {
            command.CountryCode = countrycode;
            return Ok(await Mediator.Send(command));
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Get occupations" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OccupationResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("occupations/country/{countrycode}")]
        public async Task<IActionResult> GetOccupations([FromRoute] string countrycode)
        {
            var command = new GetOccupationQuery { CountryCode= countrycode };
            var res = await Mediator.Send(command);
            return Ok(res);
        }
    }
}