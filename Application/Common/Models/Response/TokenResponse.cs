using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response
{
    public class LoginResponse
    {
        public Token Token { get; set; }
        public User User { get; set; }
    }
    public class Token    {
        public string Access_Token { get; set; }
        public DateTime ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshToken_ExpiresIn { get; set; }
    }


    public class User
    {
        public string firstName { get; set; }
        public string lastName { get;set; }
        public string middleName { get; set; }
        public string lastLogin { get; set; }
        public string localBankCode { get; set; }
        public string photoUrl { get; set; }
        public bool hasUploadedKYC { get; set; }
        public string cifId { get; set; }
        public string phoneNumber { get; set; }
        public string emailAddress { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string dateOfBirth { get; set; }
        public string customerId { get; set; }
        public string accountNumber { get; set; }
        public string deviceId { get; set; }
    }

}
