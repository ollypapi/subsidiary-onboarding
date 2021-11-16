using Domain.Enum;
using System;

namespace Domain.Entities
{
    public class Verification : BaseEntity
    {
        public string OtpCode { get; set; }
        public string TrackingCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime ExpiryDate { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public OtpPurpose Purpose { get; set; }
        public OtpStatus Status { get; set; }
        public Nullable<DateTime> DateUsed { get; set; }
       

    }
}