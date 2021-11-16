using Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Common.Models
{
    public class IdentificationModel: DocumentBase
    {
        public string IdentificationNumber { get; set; }
        public IdentificationEnum IdentificationType { get; set; }
    }
    public class DocumentModel:DocumentBase
    {
       
        public DocumentTypeEnum DocumentType { get; set; }
    }
    public class DocumentBase
    {
        public string DocumentName { get; set; }
        [JsonIgnore]
        public string DocumentFolder { get; set; }
    }
}
