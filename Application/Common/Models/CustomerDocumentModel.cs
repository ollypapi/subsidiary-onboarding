using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class CustomerDocumentModel
    {
        public long Id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
        public string? DocumentType { get; set; }
        public Identification? Identification { get; set; }
        public string IdentificationId { get; set; }
        public string IdentificationNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public long CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
