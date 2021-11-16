using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class SendMailModel
    {
        public string AppCode { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Template { get; set; }
    }
}
