namespace Application.Common.Models.Requests.FIBServiceRequests
{
    public class CustomerByAccountNoRequest : FIBaseRequest
    {
        public string AccountNumber { get; set; }
    }
}
