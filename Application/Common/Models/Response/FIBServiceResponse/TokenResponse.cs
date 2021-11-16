using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response.FBNServiceResponse
{

    public class TokenResponse : TokenBasics
    {
        public string RefreshToken { get; set; }
    }
    public class TokenBasics
    {
        public string Access_Token { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
