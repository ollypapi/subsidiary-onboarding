using Application.Common.Enums;
using Application.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class DeviceException : Exception
    {
        public DeviceException()
            : base()
        {
        }

        public DeviceException(string message)
            : base(message)
        {
        }

        public DeviceException(string message, string ResponseCode)
          : base(message)
        {
            this.Data.Add("ResponseCode", ResponseCode);
        }

        public DeviceException(string message, string ResponseCode, string entityId)
         : base(message)
        {
            this.Data.Add("ResponseCode", ResponseCode);
            this.Data.Add("EntityId", entityId);
        }

        public DeviceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DeviceException(string name, object key) : base($"Requested \"{name}\" ({key}) resulted into error")
        {
        }

        public bool Status { get; set; } = false;
        public string ResponseCode { get; set; } = ResponseCodeEnum.DeviceValidationFailed.GetDescription();
    }
    
}
