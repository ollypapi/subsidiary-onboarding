using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.AccountService
{
    public class AccountDetailRequest: FIBaseRequest
    {
        public string AccountNumber { get; set; }
    }
}
