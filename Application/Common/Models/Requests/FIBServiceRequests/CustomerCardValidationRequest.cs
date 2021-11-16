namespace Application.Common.Models.Requests.FIBServiceRequests
{
    public class CustomerCardValidationRequest : FIBaseRequest
    {
        public string MobileNumber { get; set; }
        public string AccountNumber { get; set; }
        public string CardPan { get; set; }
        public string Pin { get; set; }
    }
}
