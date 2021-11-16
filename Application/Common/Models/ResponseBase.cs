using System;

namespace Application.Common.Models
{
    [Serializable]
    public class ResponseBase
    {
        public string RequestId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
