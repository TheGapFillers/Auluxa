using System.Collections.Generic;
using Auluxa.WebApp.Devices.Models;
using Newtonsoft.Json;

namespace Auluxa.WebApp.Zones.Models
{
    public class Zone
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<Device> Devices { get; set; } = new List<Device>();
    }
}
