using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response
{
    public class OtpResponse
    {
        public string TrackingId { get; set; }
        public string OtpCode { get; set; }
    }
}
