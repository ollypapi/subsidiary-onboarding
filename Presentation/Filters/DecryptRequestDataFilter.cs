using Application.Common.Models;
using Application.Helper;
using Infrastructure.ServiceBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Presentation.Filters
{
    public class DecryptRequestDataFilter<T> : IAsyncActionFilter
    {
        public T RequestDataType { get; set; }
        private const string REQUEST = "data";
        private readonly ILogger<DecryptRequestDataFilter<T>> _logger;
        private readonly BaseUtility _baseSettings;

        public DecryptRequestDataFilter(ILogger<DecryptRequestDataFilter<T>> logger, BaseUtility baseSettings)
        {
            _logger = logger;
            _baseSettings = baseSettings;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.TryGetValue(REQUEST, out var output))
            {
                context.Result = new ObjectResult(ResponseErrorModel.Failure("Invalid Payload Error", "01"))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            var request = output as EncryptedRequestModel;

            _logger.LogInformation($"{typeof(T).Name} ENCRYPTED REQUEST ==> {Util.SerializeAsJson(request)}");
            if (!request.IsValid(out string problemSource))
            {
                context.Result = new ObjectResult(ResponseErrorModel.Failure($"{"Invalid Payload error"} - {problemSource}", "01"))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            if (!Util.IsBase64String(request.EncryptedData))
            {
                context.Result = new ObjectResult(ResponseErrorModel.Failure("Parameter is not a base64 string", "02"))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                return;
            }

            try
            {
                var deserializedData = Util.DecryptRequest<T>(request.EncryptedData, _baseSettings.EncryptionKey);
                if (deserializedData != null)
                {
                    context.ActionArguments["command"] = deserializedData;
                }
                await next();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Decryption error");
                var error = ResponseErrorModel.Failure("Decryption Error","03");
                context.Result = new ObjectResult(error)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
