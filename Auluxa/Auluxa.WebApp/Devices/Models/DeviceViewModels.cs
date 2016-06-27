using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auluxa.WebApp.Devices.Models
{
    public class CreateDeviceViewModel
    {
        public int DeviceModelId { get; set; }
    }

    public class UpdateDeviceSettingsViewModel
    {
        public int DeviceId { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }

    public class UpdateDeviceZonesViewModel
    {
        public int DeviceId { get; set; }
        public string zoneIds { get; set; }
    }
}