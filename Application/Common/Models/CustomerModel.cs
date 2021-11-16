using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class CustomerModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string CifId { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string CustomerId { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ReferralCode { get; set; }
        public string ReferredBy { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Town { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string MaritalStatus { get; set; }
        public string Branch { get; set; }
        public string DeviceId { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public RegistrationStage Stage { get; set; }
        public string PhoneNumber { get; set; }
        public string SubsidiaryId { get; set; }
        public string IdType { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
    }
}
