using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserActivity
    {
        [Key]
        public long Id { get; set; }
        public string CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public string Activity { get; set; }
        public string ActivityResult { get; set; }
        public string ResultDescription { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Path { get; set; }
        public string IPAddress { get; set; }
        public Guid ActivityTrackerId { get; set; } = Guid.NewGuid();
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
