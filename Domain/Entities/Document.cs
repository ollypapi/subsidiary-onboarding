using Domain.Enum;
using System;

namespace Domain.Entities
{
    public class Document:BaseEntity
    {
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
        public string DocumentFolder { get; set; }
        public DocumentType? DocumentType { get; set; }
        public Identification? Identification { get; set; }
        public string IdentificationId { get; set; }
        public string  IdentificationNumber { get; set; }
        public DocumentStatus? Status { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}