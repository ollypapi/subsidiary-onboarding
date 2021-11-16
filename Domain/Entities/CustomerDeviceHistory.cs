using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class DeviceHistory : BaseEntity
    {
        public long CustomerDeviceId { get; set; }
        public DeviceHistoryEnum Description { get; set; }
        public CustomerDevice CustomerDevice { get; set; }
    }
}
