using App.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Application.Common.Models
{
    public class TokenDataModel
    {
        public long CustomerId { get; set; }
        public string UserName { get; set; }
        public string MobileNumber { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CIF { get; set; }
        public JwtUserType JwtUserType { get; set; } = JwtUserType.AnonynmousUser;
    }

    public class UserData
    {
        public long CustomerId { get; set; }
        public string UserType { get; set; }
        public string MobileNumber { get; set; }
        public string CustomerName { get; set; }
        public string UserEmail { get; set; }

    }

    public enum JwtUserType
    {
        [Description("Registered")]
        RegisteredUser,
        [Description("Anonymous")]
        AnonynmousUser
    }
}
