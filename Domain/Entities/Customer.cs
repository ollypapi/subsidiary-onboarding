using Domain.Enum;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Customer:BaseEntity
    {
        public Customer()
        {
            Documents = new HashSet<Document>();
            SecurityQuestions = new HashSet<SecurityQuestion>();
            Verifications = new HashSet<Verification>();
        }
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
        public string Town  { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string MaritalStatus { get; set; }
        public string Branch { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] TransactionPin { get; set; }
        public string DeviceId { get; set; }
        public byte[] TransactionPinSalt { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public RegistrationStage Stage { get; set; }
        public bool IsExistingCustomer { get; set; }
        public string PhoneNumber { get; set; }
        public string SubsidiaryId { get; set; }
        public string IdType { get; set; }
        public string Status { get; set; }
        public string CountryId { get; set; }
        public bool IsLogin { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefereshTokenExpiresIn { get; set; }
        public int LoginAttemptAccount { get; set; }


        public ICollection<DeviceHistory> DeviceHistories { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Verification> Verifications { get; set; }
        public ICollection<SecurityQuestion> SecurityQuestions { get; set; }
    }


}
