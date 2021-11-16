using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Common.Models.StatementServive
{
    public class GetNTransactionRequest: FIBaseRequest
    {
        public string AccountNumber { get; set; }
        [JsonIgnore]
        public string MobileNumber { get { return ""; } }
        [JsonIgnore]
        public bool SendSms {
            get
            {
                return false;
            }
        }
        [JsonIgnore]
        public int TransactionsCount { get { return 5; } }

    }
}
