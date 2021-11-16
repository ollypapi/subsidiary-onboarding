using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response.FIBServiceResponse
{
    public class AccountDetailsResponse
    {
        public string CifId { get; set; }
        public string CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public bool IsCreditFrozen { get; set; }
        public string FreezeCode { get; set; }
        public string ProductCode { get; set; }
        public string Product { get; set; }
        public string AccountStatus { get; set; }
        public string CurrencyCode { get; set; }
        public string BranchCode { get; set; }
        public string Branch { get; set; }
        public decimal BookBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal LienAmount { get; set; }
        public decimal UnclearedBalance { get; set; }
        public decimal ProductMinimumBalance { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string RelationshipManagerId { get; set; }
        public string  FinaclePhoneNumber { get; set; }
        public string FinacleEmail { get; set; }
        public string AlertPhoneNumber { get; set; }
        public string AlertEmail { get; set; }
        public string RequestId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
}
