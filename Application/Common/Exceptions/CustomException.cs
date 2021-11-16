using Application.Common.Enums;
using Application.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException()
            : base()
        {
        }

        public CustomException(string message)
            : base(message)
        {
          
        }

        public CustomException(string message, string responseCode)
           : base(message)
        {
            this.Data.Add("ResponseCode", responseCode);
        }

        public CustomException(string message, string responseCode, string entityId)
          : base(message)
        {
            this.Data.Add("ResponseCode", responseCode);
            this.Data.Add("EntityId", entityId);
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public CustomException(string name, object key) : base($"Requested \"{name}\" ({key}) resulted into error")
        {
        }

        public bool Status { get; set; } = false;
        public string ResponseCode { get; set; } = ResponseCodeEnum.ApplicationError.GetDescription();
    }
}
