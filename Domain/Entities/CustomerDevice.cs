using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class CustomerDevice:BaseEntity
    {
        public CustomerDevice()
        {
            this.DeviceHistories = new HashSet<DeviceHistory>();
        }
        public string DeviceId { get; set; }
        public string OS { get; set; }
        public string DeviceName { get; set; }
        public string Status { get; set; }
        public long CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<DeviceHistory> DeviceHistories { get; set; }
    }
}
