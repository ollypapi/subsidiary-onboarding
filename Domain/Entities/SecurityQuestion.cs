namespace Domain.Entities
{
    public class SecurityQuestion:BaseEntity
    {
        public string  Question { get; set; }
        public string Answer { get; set; }
        public long CustomerId { get; set; }
        public string Status { get; set; }
        public Customer Customer { get; set; }

    }
}