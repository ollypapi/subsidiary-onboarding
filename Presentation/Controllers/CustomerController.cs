using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Handlers.Commands.CustomerCommands;
using Application.Handlers.Commands.OTPCommands;
using Application.Handlers.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Customer information" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerModel>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("country/{countrycode}"), CustomAuthorize]
        public async Task<IActionResult> GetCustomer([FromRoute] string countrycode)
        {
            var command = new GetCustomersQuery { CountryCode = countrycode };
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Customer with pending documents" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerModel>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("with-pending-documents/country/{countrycode}"), CustomAuthorize]
        public async Task<IActionResult> GetCustomerWithPendingDocuments([FromRoute] string countrycode)
        {
            var command = new GetCustomerWithPendingDocumentQuery { CountryCode = countrycode };
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Customer documents" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerDocumentModel>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("account/{accountNumber}/documents"), CustomAuthorize]
        public async Task<IActionResult> GetCustomerDocuments([FromRoute] string accountNumber)
        {
            var command = new GetCustomerDocumentsQuery { AccountNumber = accountNumber };
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Customer device" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<CustomerDeviceModel>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("{customerId}/devices"), CustomAuthorize]
        public async Task<IActionResult> GetCustomerDevices([FromRoute] long customerId)
        {
            var command = new GetCustomerDevicesQuery { CustomerId = customerId };
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Customer documents update" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("update/document"), CustomAuthorize]
        public async Task<IActionResult> UpdateDocument( [FromBody] UpdateDocumentCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Documents update status" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("update/document/status"), CustomAuthorize]
        public async Task<IActionResult> UpdateDocumentStatus( [FromBody] UpdateDocumentStatusCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Device update" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("device/status/update"), CustomAuthorize]
        public async Task<IActionResult> UpdateDevice( [FromBody] UpdateDeviceCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Customer by account number information" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseModel<CustomerModel>), (int)HttpStatusCode.OK)]
        [HttpGet, Route("accountnumber/{accountnumber}/country/{Countrycode}"), CustomAuthorize]
        public async Task<IActionResult> GetCustomerByAccountNumber([FromRoute] string accountnumber, [FromRoute] string Countrycode)
        {
            var command = new GetCustomerByAccountNumberQuery { AccountNumber = accountnumber,Countrycode= Countrycode };
            var res = await Mediator.Send(command);
            return Ok(res);
        }


       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Customer status" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("profile/status/update"), CustomAuthorize]
        public async Task<IActionResult> UpdateCustomerStatus( [FromBody] UpdateCustomerStatusCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Admin Generate Token to Update security questions" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("admin/initiate/security-question/update"), CustomAuthorize]
        public async Task<IActionResult> AdminInitiateSecurityQuestionUpdate([FromBody] InitiateResetSecurityQuestionsOTPCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }


        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Initial security questions" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<OtpResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("initiate/security-question/update"), CustomAuthorize]
        public async Task<IActionResult> InitiateSecurityQuestionUpdate([FromBody] InitiateResetSecurityQuestionsOTPCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Security questions update" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("update/security-question"), CustomAuthorize]
        public async Task<IActionResult> UpdateSecurityQuestion( [FromBody] UpdateSecurityQuestionCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Security questions update" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("reset/security-question")]
        public async Task<IActionResult> ResetSecurityQuestion([FromBody] ResetSecurityQuestionCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
    }
}
