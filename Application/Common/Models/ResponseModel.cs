using Application.Common.Enums;
using Application.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string ResponseCode { get; set; }


        public static ResponseModel Success(string message = null)
        {
            return new ResponseModel()
            {
                Status = true,
                Message = message ?? "Request was Successful",
                ResponseCode = ResponseCodeEnum.Success.GetDescription()
            };
        }

        public static ResponseModel Failure(string message = null,string responseCode = null ) // , Dictionary<string, string> errors = null
        {
            return new ResponseModel()
            {
                Message = message ?? "Request was not completed",
                ResponseCode = responseCode == null ? ResponseCodeEnum.ApplicationError.GetDescription() : responseCode
                //Errors = errors
            };
        }
        public static ResponseModel ValidationError(string message = null) // , Dictionary<string, string> errors = null
        {
            return new ResponseModel()
            {
                Message = message ?? "One or more validation error occurred",
                ResponseCode = ResponseCodeEnum.ValidationError.GetDescription()
                //Errors = errors
            };

        }
    }


   public class EntityResponseModel: ResponseModel
    {
        public string EntityId { get; set; }

        public static EntityResponseModel Failure(string message = null, string responseCode = null, string entityId=null) // , Dictionary<string, string> errors = null
        {
            return new EntityResponseModel()
            {
                Message = message ?? "Request was not completed",
                ResponseCode = responseCode == null ? ResponseCodeEnum.ApplicationError.GetDescription() : responseCode,
                EntityId = entityId,
                Status = false
                //Errors = errors
            };

        }
    }


    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }

        public static ResponseModel<T> Success(T data, string message = null)
        {
            return new ResponseModel<T>()
            {
                Status = true,
                Message = message ?? "Request was Successful",
                Data = data,
                ResponseCode = ResponseCodeEnum.Success.GetDescription()
            };
        }
    }

    public class ResponseErrorModel : ResponseModel
    {
        public IDictionary<string, string[]> Errors { get; set; }

        public static ResponseModel Failure(IDictionary<string, string[]> errors = null, string message = null)
        {
            return new ResponseErrorModel()
            {
                Message = message ?? "Request was not completed",
                Errors = errors ?? new Dictionary<string, string[]>(),
                ResponseCode = ResponseCodeEnum.Success.GetDescription()
            };
        }
    }
}
