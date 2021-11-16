using System;

namespace Application.Common.Models
{
    public class DeviceModel
    {
        public long Id { get; set; }
        public string DeviceId { get; set; }
        public string OS { get; set; }
        public string DeviceName { get; set; }
    }

    public class CustomerDeviceModel: DeviceModel
    {
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
