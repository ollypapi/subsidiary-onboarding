using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Handlers.Commands.Setting;
using Application.Handlers.Queries.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize]
    public class SettingsController : BaseController
    {
       
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<long>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("add/number-of-devices/country/{countrycode}")]
        public async Task<IActionResult> AddNumberOfDevice([FromBody] AddSettingConfigurationCommand command, [FromRoute] string countrycode)
        {
            command.SettingType = Domain.Enum.SettingEnum.MaxDeviceCount;
            command.CountryCode = countrycode;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

       
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("update/number-of-devices/country/{countrycode}")]
        public async Task<IActionResult> UpdateNumberofDevice([FromBody] UpdateSettingConfigurationCommand command, [FromRoute] string countrycode)
        {
            command.CountryCode = countrycode;
            command.SettingType = Domain.Enum.SettingEnum.MaxDeviceCount;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<ApplicationSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("{id}/number-of-devices")]
        public async Task<IActionResult> GetNumberofDevice([FromRoute] long id)
        {
            var query = new GetSettingConfigurationByIdQuery { Id= id };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<ApplicationSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("number-of-devices/country/{countrycode}")]
        public async Task<IActionResult> GetNumberofDeviceByCountryCode([FromRoute] string countrycode)
        {
            var query = new GetSettingConfigurationQuery { CountryCode = countrycode, SettingType = Domain.Enum.SettingEnum.MaxDeviceCount };
            var res = await Mediator.Send(query);
            return Ok(res);
        }


        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<long>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("add/scheme-of-code/country/{countrycode}")]
        public async Task<IActionResult> AddSchemeOfCode([FromBody] AddSchemeCodeCommand command, [FromRoute] string countrycode)
        {
            command.SettingType = Domain.Enum.SettingEnum.SchemeCode;
            command.CountryCode = countrycode;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("update/scheme-of-code/country/{countrycode}")]
        public async Task<IActionResult> UpdateSchemeCode([FromBody] UpdateSchemeCodeConfiguration command, [FromRoute] string countrycode)
        {
            command.SettingType = Domain.Enum.SettingEnum.SchemeCode;
            command.CountryCode = countrycode;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<SchemeCodeSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("{id}/scheme-of-code")]
        public async Task<IActionResult> GetSchemeCode([FromRoute] long id)
        {
            var query = new GetSchemeCodeQuery { Id = id };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<SchemeCodeSettingResponse>>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("scheme-of-code/country/{countrycode}")]
        public async Task<IActionResult> GetSchemeCodeByCountryCode([FromRoute] string countrycode)
        {
            var query = new GetSchemeCodesQuery { CountryCode = countrycode, SettingType = Domain.Enum.SettingEnum.SchemeCode };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<long>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("add/maximum-login-attempt-count/country/{countrycode}")]
        public async Task<IActionResult> AddLoginAttemptCount([FromBody] AddSettingConfigurationCommand command, [FromRoute] string countrycode)
        {
            command.SettingType = Domain.Enum.SettingEnum.PasswordRetryMaxCount;
            command.CountryCode = countrycode;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

  
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("update/maximum-login-attempt-count/country/{countrycode}")]
        public async Task<IActionResult> UpdateMaximumLoginAttemptCount([FromBody] UpdateSettingConfigurationCommand command, [FromRoute] string countrycode)
        {
            command.CountryCode = countrycode;
            command.SettingType = Domain.Enum.SettingEnum.MaxConcurrentLogin;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<ApplicationSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("{id}/maximum-login-attempt-count")]
        public async Task<IActionResult> GetMaximumLoginAttemptCount([FromRoute] long id)
        {
            var query = new GetSettingConfigurationByIdQuery { Id = id };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<ApplicationSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("maximum-login-attempt-count/country/{countrycode}")]
        public async Task<IActionResult> GetMaximumLoginAttemptCountByCountryCode([FromRoute] string countrycode)
        {
            var query = new GetSettingConfigurationQuery { CountryCode = countrycode, SettingType = Domain.Enum.SettingEnum.PasswordRetryMaxCount };
            var res = await Mediator.Send(query);
            return Ok(res);
        }



        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<long>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("add/maximum-transaction-history-count/country/{countrycode}")]
        public async Task<IActionResult> AddTransactionHistoryCountCount([FromBody] AddSettingConfigurationCommand command, [FromRoute] string countrycode)
        {
            command.SettingType = Domain.Enum.SettingEnum.TransactionHistoryCount;
            command.CountryCode = countrycode;
            var res = await Mediator.Send(command);
            return Ok(res);
        }


        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("update/maximum-transaction-history-count/country/{countrycode}")]
        public async Task<IActionResult> UpdateMaximumTransactionHistoryCount([FromBody] UpdateSettingConfigurationCommand command, [FromRoute] string countrycode)
        {
            command.CountryCode = countrycode;
            command.SettingType = Domain.Enum.SettingEnum.TransactionHistoryCount;
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<ApplicationSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("{id}/maximum-transaction-history-count")]
        public async Task<IActionResult> GetMaximumTransactionHistoryCount([FromRoute] long id)
        {
            var query = new GetSettingConfigurationByIdQuery { Id = id };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<ApplicationSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpGet, Route("maximum-transaction-history-count/country/{countrycode}")]
        public async Task<IActionResult> GetMaximumTransactionHistoryCountByCountryCode( [FromRoute] string countrycode)
        {
            var query = new GetSettingConfigurationQuery { CountryCode = countrycode, SettingType = Domain.Enum.SettingEnum.TransactionHistoryCount };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<ApplicationSettingResponse>), (int)HttpStatusCode.OK)]
        [Produces("application/json")]
        [HttpPost, Route("{id}/country/{countrycode}/delete")]
        public async Task<IActionResult> deleteSetting([FromRoute] string countrycode, [FromRoute] long id)
        {
            var query = new DeleteSettingConfigurationCommand { CountryCode = countrycode, Id = id };
            var res = await Mediator.Send(query);
            return Ok(res);
        }

    }
}
