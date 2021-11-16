using System;

namespace Application.Common.Models
{
    public class FIBaseRequest
    {
        public string RequestId => Guid.NewGuid().ToString("N");
        public string CountryId { get; set; }
    }
    public class BranchModel : FIBaseRequest        
    {}
}
