using System.Collections.Generic;

namespace Application.Common.Models.Response.FIBServiceResponse
{
    public class CustomerByMobileResponse : ResponseBase
    {
        public List<FICustomer> Customers { get; set; }
    }

    public class FICustomer
    {
        public string CustomerId { get; set; }
        public string CifId { get; set; }
    }
}