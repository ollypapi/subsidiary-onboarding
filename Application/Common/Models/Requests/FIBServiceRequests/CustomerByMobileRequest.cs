namespace Application.Common.Models.Requests.FIBServiceRequests
{
    public class CustomerByMobileRequest : FIBaseRequest
    {
        public string MobileNumber { get; set; }
    }
}
