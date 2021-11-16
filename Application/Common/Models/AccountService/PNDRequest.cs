using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.AccountService
{
    public class PNDRequest: FIBaseRequest
    {
        public string AccountNumber { get; set; }
        public string FreezeCode => Guid.NewGuid().ToString("N");
        public string FreezeReason { get; set; }
        public string ClientReferenceId => Guid.NewGuid().ToString("N");
    }
}
