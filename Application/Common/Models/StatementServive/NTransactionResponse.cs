using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.StatementServive
{
    public class NTransactionResponse : ResponseBase
    {
        public List<Transaction> LastNTransactions { get; set; }
        public bool IsSendSms { get; set; }

    }

    public class Transaction
    { 
        public double Amount { get; set; }
        public string TransactionType { get; set; }
        public string Currency { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Remarks { get; set; }
    }
}
