using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Requests.FIBServiceRequests
{
    public class AccountDetailByAccountNoRequest: FIBaseRequest
    {
        public string AccountNumber { get; set; }
    }
}
