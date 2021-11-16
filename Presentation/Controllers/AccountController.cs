using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Common.Models.StatementServive;
using Application.Handlers.Commands.CustomerCommands;
using Application.Handlers.Queries;
using Application.Handlers.Queries.Statement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Transactions" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<NTransactionResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("{accountnumber}/transactions/countryId/{countryId}"), CustomAuthorize]
        public async Task<IActionResult> GetNTransactions( [FromRoute] string accountnumber, string countryId)
        {
            GetNTransactionsQuery query = new GetNTransactionsQuery { AccountNumber = accountnumber,CountryId= countryId };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Account details" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<AccountDetailResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("{accountnumber}/countryId/{countryId}"), CustomAuthorize]
        public async Task<IActionResult> GetAccountDetail([FromRoute] string accountnumber, string countryId)
        {
            GetAccountDetailQuery query = new GetAccountDetailQuery { AccountNumber = accountnumber, CountryId = countryId };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Place no Debit" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<AccountDetailResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("place-post-no-debit"), CustomAuthorize]
        public async Task<IActionResult> PlaceNoDebit([FromBody] PlacePostNoDebitCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [TypeFilter(typeof(ActivityLogFilter), Arguments = new object[] { "Remove Place no Debit" })]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<AccountDetailResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("remove-post-no-debit"), CustomAuthorize]
        public async Task<IActionResult> RemoveNoDebit([FromBody] RemovePlaceNoDebitCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
    }
}
