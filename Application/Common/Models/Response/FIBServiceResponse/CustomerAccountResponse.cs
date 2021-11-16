using System;

namespace Application.Common.Models.Response.FIBServiceResponse
{
    public class CustomerAccountResponse : ResponseBase
    {
        public string CifId { get; set; }
        public string CustomerId { get; set; }
        public string Title { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CustomerCategory { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Stage { get; set; }
        public string AccountNumber { get; set; }
        public bool IsExistingCustomer { get; set; } 
    }
}